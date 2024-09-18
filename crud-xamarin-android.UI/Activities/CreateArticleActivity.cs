using Android.App;
using Android.Content;
using Android.Content.PM;
using Android.Icu.Text;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using AndroidX.AppCompat.App;
using AndroidX.Core.Content;
using AndroidX.RecyclerView.Widget;
using crud_xamarin_android.Core.Helpers;
using crud_xamarin_android.Core.Models;
using crud_xamarin_android.Core.Services;
using crud_xamarin_android.UI.Adapters;
using crud_xamarin_android.UI.Helpers;
using Java.Util;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace crud_xamarin_android.UI.Activities
{
    [Activity(Label = "")]
    public class CreateArticleActivity : AppCompatActivity
    {
        Button btnAccept, btnCancel;
        Spinner spnCategories;
        ImageView imgArticle;
        TextView txtDeleteImage;

        ArticleService articleService;
        CategoryService categoryService;
        List<Category> categories;
        Category categorySelected;
        Java.IO.File photoFile;

        public CreateArticleActivity()
        {
            articleService = new ArticleService();
            categoryService = new CategoryService();
            categories = categoryService.GetCategories().ToList();
            categories = categories.OrderBy(c => c.Name).ToList();
        }

        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);

            SetContentView(Resource.Layout.activity_create_article);

            SupportActionBar.SetDisplayHomeAsUpEnabled(true);
            SupportActionBar.SetHomeButtonEnabled(true);

            btnAccept = FindViewById<Button>(Resource.Id.btnAceptar);
            btnAccept.Click += BtnAccept_Click;

            btnCancel = FindViewById<Button>(Resource.Id.btnCancelar);
            btnCancel.Click += BtnCancel_Click;

            imgArticle = FindViewById<ImageView>(Resource.Id.imgArticle);
            imgArticle.Click += ImgArticle_Click;

            txtDeleteImage = FindViewById<TextView>(Resource.Id.txtDeleteImage);
            txtDeleteImage.Visibility = ViewStates.Gone;
            txtDeleteImage.Click += TxtDeleteImage_Click;

            spnCategories = FindViewById<Spinner>(Resource.Id.spnCategories);
            var categories = categoryService.GetCategories().OrderBy(c=>c.Name).ToList();
            List<string> spnDataSource;
            ArrayAdapter spnAdapter;

            if (categories.Count > 0)
            {
                spnDataSource = categories.Select(c => c.Name).ToList();
            }
            else
            {
                spnDataSource = new List<string> { "(NO CATEGORIES AVAILABLE)" };
                spnCategories.Enabled = false;
            }

            spnAdapter = new ArrayAdapter<string>(this, Android.Resource.Layout.SimpleSpinnerItem, spnDataSource);
            spnAdapter.SetDropDownViewResource(Android.Resource.Layout.SimpleSpinnerDropDownItem);
            spnCategories.Adapter = spnAdapter;

            spnCategories.ItemSelected += SpnCategories_ItemSelected;
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

            if (CameraHelper.CheckResultCamera(requestCode,resultCode))
            {
                imgArticle.SetImageURI(Android.Net.Uri.Parse(photoFile.AbsolutePath));
                txtDeleteImage.Visibility = ViewStates.Visible;
            }

            if (GaleryHelper.CheckResultGalery(requestCode, resultCode))
            {
                if (data!=null)
                {
                    var imageUri = data.Data;
                    var bitmap = ImageHelper.GetResizedBitmap(imageUri, this);
                    imgArticle.SetImageBitmap(bitmap);
                    photoFile = ImageHelper.CreateImageFileFromUri2(this, imageUri);
                    txtDeleteImage.Visibility = ViewStates.Visible;
                }
            }
        }
        
        public override void OnRequestPermissionsResult(int requestCode, string[] permissions, [GeneratedEnum] Permission[] grantResults)
        {
            base.OnRequestPermissionsResult(requestCode, permissions, grantResults);

            if (CameraHelper.CheckCameraPermission(requestCode,grantResults))
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
            var toast = Toast.MakeText(this, Resource.String.message_success_article_created, ToastLength.Short);
            toast.SetGravity(GravityFlags.Top | GravityFlags.CenterHorizontal, 0, 0);
            toast.Show();

            var inpNameArt = FindViewById<EditText>(Resource.Id.inpNameArticle);
            var inpDetailsArt = FindViewById<EditText>(Resource.Id.inpDetailsArticle);

            var article = new Article
            {
                Name = inpNameArt.Text,
                Details = inpDetailsArt.Text,
                CategoryId = (categorySelected != null ? categorySelected.Id : CategoryHelper.ID_EMPTY_CATEGORY),
                ImagePath = photoFile != null ? photoFile.AbsolutePath : null,
                ImageData = photoFile != null ? ImageHelper.GetImageAsByteArray(photoFile.AbsolutePath) : null,
            };
            articleService.AddArticle(article);

            SetResult(Result.Ok);
            Finish();
        }

        private void BtnCancel_Click(object sender, EventArgs e)
        {
            Finish();
        }

        private void GoToCamera()
        {
            if (!CameraHelper.HasCameraPermission(this))
            {
                CameraHelper.RequestCameraPermission(this);
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
                GaleryHelper.RequestGaleryPermission(this);
            }
            else
            {
                OpenGallery();
            }
        }

        private void OpenCamera()
        {
            Intent takePictureIntent = new Intent(Android.Provider.MediaStore.ActionImageCapture);
            //if (takePictureIntent.ResolveActivity(PackageManager) != null)
            {
                photoFile = ImageHelper.CreateImageFile(this);
                if (photoFile != null)
                {
                    var photoURI = FileProvider.GetUriForFile(this, CommonHelper.GetFileProviderAuthorities(this), photoFile);
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