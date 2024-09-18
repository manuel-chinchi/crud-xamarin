using Android.App;
using Android.Content;
using Android.Content.PM;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using AndroidX.AppCompat.App;
using crud_xamarin_android.Core.Helpers;
using crud_xamarin_android.Core.Models;
using crud_xamarin_android.Core.Services;
using crud_xamarin_android.UI.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace crud_xamarin_android.UI.Activities
{
    [Activity(Label = "")]
    public class EditArticleActivity : AppCompatActivity
    {
        EditText inpNameArticle;
        EditText inpDetailsArticle;
        Spinner spnCategory;
        Button btnAccept;
        Button btnCancel;
        ImageView imgArticle;
        TextView txtNotImage;

        Article article;
        ArticleService articleService;
        List<Category> categories;
        Category categorySelected;
        Java.IO.File photoFile;

        public EditArticleActivity()
        {
            articleService = new ArticleService();
            categories = new CategoryService().GetCategories().ToList();
            categories = categories.OrderBy(c => c.Name).ToList();
        }

        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);

            SetContentView(Resource.Layout.activity_create_article);

            SupportActionBar.SetDisplayHomeAsUpEnabled(true);
            SupportActionBar.SetHomeButtonEnabled(true);

            inpNameArticle = FindViewById<EditText>(Resource.Id.inpNameArticle);
            inpDetailsArticle = FindViewById<EditText>(Resource.Id.inpDetailsArticle);
            spnCategory = FindViewById<Spinner>(Resource.Id.spnCategories);
            imgArticle = FindViewById<ImageView>(Resource.Id.imgArticle);
            txtNotImage = FindViewById<TextView>(Resource.Id.txtNoImage);
            btnAccept = FindViewById<Button>(Resource.Id.btnAceptar);
            btnCancel = FindViewById<Button>(Resource.Id.btnCancelar);

            int articleId = Intent.GetIntExtra("ArticleId", -1);
            int categoryId = Intent.GetIntExtra("CategoryId", -1);

            article = articleService.GetArticleById(articleId);
            categories = categories.OrderBy(c => c.Name).ToList();
            ArrayAdapter spnAdapter;
            List<string> spnDataSource;

            if (article != null)
            {
                inpNameArticle.Text = article.Name;
                inpDetailsArticle.Text = article.Details;

                if (article.ImageData != null)
                {
                    //var bitmap = BitmapFactory.DecodeFile(article.ImagePath); // TODO con imagenes muy grandes da error
                    var bitmap = ImageHelper.GetResizedBitmapFromBytes(article.ImageData, 1024, 1024);
                    imgArticle.SetImageBitmap(bitmap);
                    txtNotImage.Visibility = ViewStates.Gone;
                }
                else
                {
                    txtNotImage.Text = "(NOT IMAGE)";
                    txtNotImage.Visibility = ViewStates.Visible;
                }
            }

            if (categoryId == CategoryHelper.ID_EMPTY_CATEGORY && categories.Count > 0)
            {
                categories.Insert(0, new Category
                {
                    Id = CategoryHelper.ID_EMPTY_CATEGORY,
                    Name = CategoryHelper.NAME_EMPTY_CATEGORY
                });
                spnDataSource = categories.Select(c => c.Name).ToList();
                int position = categories.FindIndex(c => c.Id == categoryId);
                spnCategory.SetSelection(position);
            }
            else if (categories.Count > 0)
            {
                spnDataSource = categories.Select(c => c.Name).ToList();
                int position = categories.FindIndex(c => c.Id == categoryId);
                spnCategory.SetSelection(position);
            }
            else
            {
                spnDataSource = new List<string> { CategoryHelper.NAME_EMPTY_CATEGORY };
                spnCategory.Enabled = false;
            }

            spnAdapter = new ArrayAdapter<string>(this, Android.Resource.Layout.SimpleSpinnerItem, spnDataSource);
            spnAdapter.SetDropDownViewResource(Android.Resource.Layout.SimpleSpinnerDropDownItem);
            spnCategory.Adapter = spnAdapter;

            btnAccept.Click += BtnAccept_Click;
            btnCancel.Click += BtnCancel_Click;
            spnCategory.ItemSelected += SpnCategory_ItemSelected;
            imgArticle.Click += ImgArticle_Click;
        }

        private void SpnCategory_ItemSelected(object sender, AdapterView.ItemSelectedEventArgs e)
        {
            if (categories.Count > 0)
            {
                categorySelected = categories[e.Position];
            }
        }

        public override bool OnOptionsItemSelected(IMenuItem item)
        {
            switch (item.ItemId)
            {
                case Android.Resource.Id.Home:
                    OnBackPressed();
                    return true;
                default:
                    return base.OnOptionsItemSelected(item);
            }
        }

        protected override void OnActivityResult(int requestCode, [GeneratedEnum] Result resultCode, Intent data)
        {
            base.OnActivityResult(requestCode, resultCode, data);

            if (CameraHelper.CheckResultCamera(requestCode, resultCode))
            {
                imgArticle.SetImageURI(Android.Net.Uri.Parse(photoFile.AbsolutePath));
                txtNotImage.Visibility = ViewStates.Gone;
            }

            if (GaleryHelper.CheckResultGalery(requestCode, resultCode))
            {
                if (data != null)
                {
                    var imageUri = data.Data;

                    #region v1
                    
                    //imgArticle.SetImageURI(imageUri); // TODO si la imagen es muy grande da error
                    
                    #endregion

                    #region v2

                    var bitmap = ImageHelper.GetResizedBitmap(imageUri, this);
                    imgArticle.SetImageBitmap(bitmap);
                    photoFile = ImageHelper.CreateImageFileFromUri2(this, imageUri);
                    
                    #endregion

                    txtNotImage.Visibility = ViewStates.Gone;
                }
            }
        }

        public override void OnRequestPermissionsResult(int requestCode, string[] permissions, [GeneratedEnum] Permission[] grantResults)
        {
            base.OnRequestPermissionsResult(requestCode, permissions, grantResults);

            if (CameraHelper.CheckCameraPermission(requestCode, grantResults))
            {
                OpenCamera();
            }

            if (GaleryHelper.CheckGaleryPermission(requestCode, grantResults))
            {
                OpenGallery();
            }
        }

        private void ImgArticle_Click(object sender, EventArgs e)
        {
            string[] options = { "Take a photo", "Choose from Galery" };
            var builder = new AndroidX.AppCompat.App.AlertDialog.Builder(this);
            builder.SetTitle("Select option");
            builder.SetItems(options, (dialog, which) =>
            {
                switch (which.Which)
                {
                    case 0:
                        GoToCamera();
                        break;
                    case 1:
                        GoToGallery();
                        break;
                }
            });
            builder.Show();
        }

        private void GoToCamera()
        {
            if (!CameraHelper.HasCameraPermission(this))
            {
                CameraHelper.RequestCameraPermission(this); // TODO pone en cola el permiso para solicitar la camara
            }
            else
            {
                OpenCamera();
            }
        }

        private void GoToGallery()
        {
            if (!GaleryHelper.HasGaleryPermission(this))
            {
                GaleryHelper.RequestGaleryPermission(this); // TODO pone en cola el permiso para solicitar la camara
            }
            else
            {
                OpenGallery();
            }
        }

        private void OpenCamera()
        {
            Intent takePictureIntent = new Intent(Android.Provider.MediaStore.ActionImageCapture);
            bool hasCameraAvailable = takePictureIntent.ResolveActivity(PackageManager) != null;
            if (hasCameraAvailable)
            {
                photoFile = ImageHelper.CreateImageFile(this);
                if (photoFile != null)
                {
                    var photoURI = AndroidX.Core.Content.FileProvider.GetUriForFile(this, CommonHelper.GetFileProviderAuthorities(this), photoFile);
                    takePictureIntent.PutExtra(Android.Provider.MediaStore.ExtraOutput, photoURI);
                    StartActivityForResult(takePictureIntent, CameraHelper.REQUEST_IMAGE_CAPTURE);
                }
            }
        }

        private void OpenGallery()
        {
            Intent takeChooseItemFromGalery = new Intent(Intent.ActionPick);
            takeChooseItemFromGalery.SetType("image/*");
            StartActivityForResult(takeChooseItemFromGalery, GaleryHelper.PICK_IMAGE_REQUEST);
        }

        private void BtnAccept_Click(object sender, EventArgs e)
        {
            article.Name = inpNameArticle.Text;
            article.Details = inpDetailsArticle.Text;
            article.ImagePath = photoFile != null ? photoFile.AbsolutePath : null;
            article.ImageData = photoFile != null ? ImageHelper.GetImageAsByteArray(photoFile.AbsolutePath) : null;

            if (categories.Count > 0)
            {
                article.Category = categories.FirstOrDefault(c => c.Name == categorySelected.Name);
                article.CategoryId = categorySelected.Id;
            }
            else
            {
                article.Category = null;
                article.CategoryId = CategoryHelper.ID_EMPTY_CATEGORY;
            }

            articleService.UpdateArticle(article);

            Intent resultIntent = new Intent();
            SetResult(Result.Ok, resultIntent);
            Finish();
        }

        private void BtnCancel_Click(object sender, EventArgs e)
        {
            SetResult(Result.Canceled);
            Finish();
        }
    }
}