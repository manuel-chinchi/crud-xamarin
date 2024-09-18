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

        ArticleService articleService;
        CategoryService categoryService;
        List<Category> categories;
        Category categorySelected;
        Java.IO.File photoFile;

        const int REQUEST_CAMERA_PERMISSION = 100;
        const string FILE_PROVIDER = "com.companyname.crud_xamarin.fileprovider";
        private static readonly int REQUEST_IMAGE_CAPTURE = 1;
        private string _currentPhotoPath;

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

            if (requestCode == REQUEST_IMAGE_CAPTURE && resultCode == Result.Ok)
            {
                var imageView = FindViewById<ImageView>(Resource.Id.imgArticle);
                imageView.SetImageURI(Android.Net.Uri.Parse(_currentPhotoPath));
            }

            if (GaleryHelper.CheckResultGalery(requestCode, resultCode))
            {
                if (data!=null)
                {
                    var imageUri = data.Data;
                    var bitmap = ImageHelper.GetResizedBitmap(imageUri, this);
                    imgArticle.SetImageBitmap(bitmap);
                    photoFile = ImageHelper.CreateImageFileFromUri2(this, imageUri);
                }
            }
        }
        
        public override void OnRequestPermissionsResult(int requestCode, string[] permissions, [GeneratedEnum] Permission[] grantResults)
        {
            base.OnRequestPermissionsResult(requestCode, permissions, grantResults);

            if (requestCode == REQUEST_CAMERA_PERMISSION)
            {
                if (grantResults.Length > 0 && grantResults[0] == Android.Content.PM.Permission.Granted)
                {
                    OpenCamera();
                }
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
            if (CheckSelfPermission(Android.Manifest.Permission.Camera) != (int)Android.Content.PM.Permission.Granted)
            {
                RequestPermissions(new string[] { Android.Manifest.Permission.Camera }, REQUEST_CAMERA_PERMISSION);
            }
            else
            {
                OpenCamera();
            }
        }

        private void OpenCamera()
        {
            Intent takePictureIntent = new Intent(Android.Provider.MediaStore.ActionImageCapture);
            if (takePictureIntent.ResolveActivity(PackageManager) != null)
            {
                photoFile = ImageHelper.CreateImageFile(this);
                if (photoFile != null)
                {
                    var photoURI = FileProvider.GetUriForFile(this, FILE_PROVIDER, photoFile);
                    takePictureIntent.PutExtra(Android.Provider.MediaStore.ExtraOutput, photoURI);
                    StartActivityForResult(takePictureIntent, REQUEST_IMAGE_CAPTURE);
                }
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

        private void OpenGallery()
        {
            Intent chosseItemFromGalleryIntent = new Intent(Intent.ActionPick);
            chosseItemFromGalleryIntent.SetType("image/*");
            StartActivityForResult(chosseItemFromGalleryIntent, GaleryHelper.PICK_IMAGE_REQUEST);
        }

        private void SpnCategories_ItemSelected(object sender, AdapterView.ItemSelectedEventArgs e)
        {
            if (categories.Count > 0)
            {
                categorySelected = categories[e.Position];
            }
        }

        private byte[] GetImageAsByteArray(string path)
        {
            byte[] imageBytes = null;
            try
            {
                using (var stream = new FileStream(path, FileMode.Open, FileAccess.Read))
                {
                    using (var memoryStream = new MemoryStream())
                    {
                        stream.CopyTo(memoryStream);
                        imageBytes = memoryStream.ToArray();
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error converting image to byte[]: {ex.Message}");
            }

            return imageBytes;
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
                ImageData = photoFile != null ? GetImageAsByteArray(photoFile.AbsolutePath) : null,
            };
            articleService.AddArticle(article);

            SetResult(Result.Ok);
            Finish();
        }

        private void BtnCancel_Click(object sender, EventArgs e)
        {
            Finish();
        }
    }
}