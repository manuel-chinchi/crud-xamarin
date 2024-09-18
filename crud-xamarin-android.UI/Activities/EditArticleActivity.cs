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
        Spinner spnCategories;
        Button btnAccept;
        Button btnCancel;
        ImageView imgArticle;
        TextView txtDeleteImage;

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
            spnCategories = FindViewById<Spinner>(Resource.Id.spnCategories);
            imgArticle = FindViewById<ImageView>(Resource.Id.imgArticle);
            txtDeleteImage = FindViewById<TextView>(Resource.Id.txtDeleteImage);
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
                    txtDeleteImage.Visibility = ViewStates.Visible;
                }
                else
                {
                    txtDeleteImage.Visibility = ViewStates.Gone;
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
                spnCategories.SetSelection(position);
            }
            else if (categories.Count > 0)
            {
                spnDataSource = categories.Select(c => c.Name).ToList();
                int position = categories.FindIndex(c => c.Id == categoryId);
                spnCategories.SetSelection(position);
            }
            else
            {
                spnDataSource = new List<string> { CategoryHelper.NAME_EMPTY_CATEGORY };
                spnCategories.Enabled = false;
            }

            spnAdapter = new ArrayAdapter<string>(this, Android.Resource.Layout.SimpleSpinnerItem, spnDataSource);
            spnAdapter.SetDropDownViewResource(Android.Resource.Layout.SimpleSpinnerDropDownItem);
            spnCategories.Adapter = spnAdapter;

            btnAccept.Click += BtnAccept_Click;
            btnCancel.Click += BtnCancel_Click;
            spnCategories.ItemSelected += SpnCategories_ItemSelected;
            imgArticle.Click += ImgArticle_Click;
            txtDeleteImage.Click += TxtDeleteImage_Click;
        }

        private void TxtDeleteImage_Click(object sender, EventArgs e)
        {
            imgArticle.SetImageResource(Resource.Drawable.ic_launcher_foreground);
            photoFile = null;
            txtDeleteImage.Visibility = ViewStates.Gone;
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
                txtDeleteImage.Visibility = ViewStates.Gone;
            }

            if (GaleryHelper.CheckResultGalery(requestCode, resultCode))
            {
                if (data != null)
                {
                    //imgArticle.SetImageURI(data.Data); // TODO si la imagen es muy grande da error
                    var imageUri = data.Data;
                    var bitmap = ImageHelper.GetResizedBitmap(imageUri, this);
                    imgArticle.SetImageBitmap(bitmap);
                    photoFile = ImageHelper.CreateImageFileFromUri2(this, imageUri);
                    txtDeleteImage.Visibility = ViewStates.Gone;
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

        private void SpnCategories_ItemSelected(object sender, AdapterView.ItemSelectedEventArgs e)
        {
            if (categories.Count > 0)
            {
                categorySelected = categories[e.Position];
            }
        }

        private void BtnAccept_Click(object sender, EventArgs e)
        {
            article.Name = inpNameArticle.Text;
            article.Details = inpDetailsArticle.Text;
            article.ImagePath = article.ImagePath!=null? article.ImagePath: (photoFile != null ? photoFile.AbsolutePath : null);
            article.ImageData = article.ImageData !=null? article.ImageData:(photoFile != null ? ImageHelper.GetImageAsByteArray(photoFile.AbsolutePath) : null);

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
            //bool hasCameraAvailable = takePictureIntent.ResolveActivity(PackageManager) != null;
            //if (hasCameraAvailable)
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
            Intent chooseItemFromGalleryIntent = new Intent(Intent.ActionPick);
            chooseItemFromGalleryIntent.SetType("image/*");
            StartActivityForResult(chooseItemFromGalleryIntent, GaleryHelper.PICK_IMAGE_REQUEST);
        }

    }
}