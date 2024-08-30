using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Widget;
using AndroidX.AppCompat.App;
using crud_xamarin_android.Core.Services;

namespace crud_xamarin_android.UI.Activities
{
    [Activity(Label = "@string/app_name", Theme = "@style/AppTheme", MainLauncher = true)]
    public class MainActivity : AppCompatActivity
    {
        Button btnArticles, btnCategories;

        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            Xamarin.Essentials.Platform.Init(this, savedInstanceState);

            SetContentView(Resource.Layout.activity_main);

            btnArticles = FindViewById<Button>(Resource.Id.btn_articles);
            btnArticles.Click += BtnArticles_Click;

            btnCategories = FindViewById<Button>(Resource.Id.btn_categories);
            btnCategories.Click += BtnCategories_Click;
        }

        public override void OnRequestPermissionsResult(int requestCode, string[] permissions, [GeneratedEnum] Android.Content.PM.Permission[] grantResults)
        {
            Xamarin.Essentials.Platform.OnRequestPermissionsResult(requestCode, permissions, grantResults);

            base.OnRequestPermissionsResult(requestCode, permissions, grantResults);
        }

        private void BtnCategories_Click(object sender, System.EventArgs e)
        {
            var intent = new Intent(this, typeof(CategoryActivity));
            StartActivity(intent);
        }

        private void BtnArticles_Click(object sender, System.EventArgs e)
        {
            var intent = new Intent(this, typeof(ArticleActivity));
            StartActivity(intent);
        }
    }
}