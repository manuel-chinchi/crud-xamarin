using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Widget;
using AndroidX.AppCompat.App;
using crud_xamarin.Core.Services;

namespace crud_xamarin
{
    [Activity(Label = "@string/app_name", Theme = "@style/AppTheme", MainLauncher = true)]
    public class MainActivity : AppCompatActivity
    {
        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            Xamarin.Essentials.Platform.Init(this, savedInstanceState);

            SetContentView(Resource.Layout.activity_main);

            #region test article view

            Button btnArticles = FindViewById<Button>(Resource.Id.btn_articles);

            //btnArticles.Click += delegate { StartActivity(typeof(ArticleActivity)); };
            btnArticles.Click += BtnArticles_Click;

            #endregion
        }

        private void BtnArticles_Click(object sender, System.EventArgs e)
        {
            var intent = new Intent(this, typeof(ArticleActivity));
            StartActivity(intent);
        }

        public override void OnRequestPermissionsResult(int requestCode, string[] permissions, [GeneratedEnum] Android.Content.PM.Permission[] grantResults)
        {
            Xamarin.Essentials.Platform.OnRequestPermissionsResult(requestCode, permissions, grantResults);

            base.OnRequestPermissionsResult(requestCode, permissions, grantResults);
        }
    }
}