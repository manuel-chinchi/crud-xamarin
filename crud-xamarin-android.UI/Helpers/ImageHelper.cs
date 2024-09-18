using Android.App;
using Android.Content;
using Android.Graphics;
using Android.Icu.Text;
using Android.OS;
using Android.Provider;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using Java.Util;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace crud_xamarin_android.UI.Helpers
{
    public static class ImageHelper
    {
        public static byte[] GetImageAsByteArray(string path)
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

        public static Java.IO.File CreateImageFile(Activity context)
        {
            // Crear un nombre de archivo único
            string timeStamp = new SimpleDateFormat("yyyyMMdd_HHmmss").Format(new Date());
            string imageFileName = "JPEG_" + timeStamp + "_";
            Java.IO.File storageDir = context.GetExternalFilesDir(Android.OS.Environment.DirectoryPictures);

            // Crear un archivo temporal de imagen
            Java.IO.File image = Java.IO.File.CreateTempFile(
                imageFileName,  /* prefijo */
                ".jpg",         /* sufijo */
                storageDir      /* directorio */
            );

            // Guardar la ruta del archivo para usarla después
            //_currentPhotoPath = image.AbsolutePath;
            return image;
        }

        //public static Java.IO.File CreateImageFileFromUri(Activity context, Android.Net.Uri uri)
        //{
        //    try
        //    {
        //        // Obtener el nombre del archivo desde el Uri
        //        string fileName = "JPEG_" + new SimpleDateFormat("yyyyMMdd_HHmmss").Format(new Date()) + ".jpg";
        //        Java.IO.File storageDir = context.GetExternalFilesDir(Android.OS.Environment.DirectoryPictures);

        //        // Crear el archivo de imagen en el directorio correspondiente
        //        Java.IO.File imageFile = new Java.IO.File(storageDir, fileName);

        //        // Abrir el InputStream desde el Uri
        //        using (var inputStream = context.ContentResolver.OpenInputStream(uri))
        //        {
        //            if (inputStream != null)
        //            {
        //                // Guardar el contenido del archivo en el nuevo archivo de imagen
        //                using (var outputStream = new Java.IO.FileOutputStream(imageFile))
        //                {
        //                    //inputStream.CopyTo(outputStream);
        //                    inputStream.CopyTo(outputStream);
        //                }
        //            }
        //        }

        //        return imageFile;
        //    }
        //    catch (Exception ex)
        //    {
        //        // Manejar excepciones
        //        //Log.Error("ImageHelper", $"Error al crear el archivo de imagen desde Uri: {ex.Message}");
        //        return null;
        //    }
        //}

        public static Java.IO.File CreateImageFileFromUri2(Activity context, Android.Net.Uri uri)
        {
            try
            {
                // Obtener el nombre del archivo desde el Uri
                string fileName = "JPEG_" + new SimpleDateFormat("yyyyMMdd_HHmmss").Format(new Date()) + ".jpg";
                Java.IO.File storageDir = context.GetExternalFilesDir(Android.OS.Environment.DirectoryPictures);

                // Crear el archivo de imagen en el directorio correspondiente
                Java.IO.File imageFile = new Java.IO.File(storageDir, fileName);

                // Abrir el InputStream desde el Uri
                using (var inputStream = context.ContentResolver.OpenInputStream(uri))
                {
                    if (inputStream != null)
                    {
                        // Abrir un FileOutputStream para el archivo de imagen
                        using (var outputStream = new System.IO.FileStream(imageFile.AbsolutePath, System.IO.FileMode.Create))
                        {
                            // Copiar el contenido del inputStream al outputStream
                            inputStream.CopyTo(outputStream);
                        }
                    }
                }

                return imageFile;
            }
            catch (Exception ex)
            {
                // Manejar excepciones
                //Log.Error("ImageHelper", $"Error al crear el archivo de imagen desde Uri: {ex.Message}");
                return null;
            }
        }



        public static Bitmap GetResizedBitmap(Android.Net.Uri imageUri, Activity context)
        {
            var options = new BitmapFactory.Options
            {
                InJustDecodeBounds = true
            };

            // Leer las dimensiones de la imagen sin cargarla completamente en memoria
            //Stream inputStream = ContentResolver.OpenInputStream(imageUri);
            Stream inputStream = context.ContentResolver.OpenInputStream(imageUri);
            BitmapFactory.DecodeStream(inputStream, null, options);
            inputStream.Close();

            // Definir el tamaño máximo permitido
            int maxHeight = 1024;
            int maxWidth = 1024;

            // Calcular el factor de escala
            int scaleFactor = Math.Min(options.OutWidth / maxWidth, options.OutHeight / maxHeight);

            // Configurar opciones para redimensionar la imagen
            options.InJustDecodeBounds = false;
            options.InSampleSize = scaleFactor;

            // Cargar la imagen redimensionada
            inputStream = context.ContentResolver.OpenInputStream(imageUri);
            Bitmap resizedBitmap = BitmapFactory.DecodeStream(inputStream, null, options);
            inputStream.Close();

            return resizedBitmap;
        }

        public static Bitmap GetResizedBitmapFromBytes(byte[] imageData, int maxWidth, int maxHeight)
        {
            try
            {
                // Opciones para obtener el tamaño de la imagen sin cargarla en memoria
                BitmapFactory.Options options = new BitmapFactory.Options
                {
                    InJustDecodeBounds = true
                };

                // Obtener las dimensiones de la imagen sin cargarla en memoria
                BitmapFactory.DecodeByteArray(imageData, 0, imageData.Length, options);

                // Calcular el factor de escalado
                int imageWidth = options.OutWidth;
                int imageHeight = options.OutHeight;
                int scaleFactor = Math.Min(imageWidth / maxWidth, imageHeight / maxHeight);

                // Cargar la imagen redimensionada
                options.InJustDecodeBounds = false;
                options.InSampleSize = scaleFactor;

                // Ahora decodificar la imagen con el factor de escalado aplicado
                return BitmapFactory.DecodeByteArray(imageData, 0, imageData.Length, options);
            }
            catch (Exception ex)
            {
                //Log.Error("ImageHelper", $"Error al redimensionar el bitmap: {ex.Message}");
                return null;
            }
        }

        //private void SaveImage(Android.Net.Uri uri)
        //{
        //    // Abrir InputStream desde el Uri
        //    using (var inputStream = ContentResolver.OpenInputStream(uri))
        //    {
        //        // Convertir a bitmap si deseas manipular la imagen
        //        var bitmap = BitmapFactory.DecodeStream(inputStream);

        //        // Si deseas guardar la imagen en un archivo en almacenamiento interno
        //        var filePath = System.IO.Path.Combine(FilesDir.AbsolutePath, "selectedImage.jpg");
        //        using (var outputStream = new FileStream(filePath, FileMode.Create))
        //        {
        //            bitmap.Compress(Bitmap.CompressFormat.Jpeg, 100, outputStream);
        //        }

        //        // Aquí podrías guardar el path o el Bitmap en tu base de datos o donde lo necesites
        //    }
        //}

        //TODO usando Xamarin.Essentials
        //private async void GoToCameraOLD()
        //{
        //    var status = await Permissions.CheckStatusAsync<Permissions.Camera>();
        //    if (status == PermissionStatus.Granted)
        //    {
        //        OpenCamera();
        //    }
        //    else
        //    {
        //        CameraHelper.RequestCameraPermission(this);
        //    }
        //}

        public static string GetPath(Android.Net.Uri uri, Activity context)
        {
            string path = null;
            String[] projection = { MediaStore.MediaColumns.Data };
            ContentResolver cr =  context.ApplicationContext.ContentResolver;
            var metaCursor = cr.Query(uri, projection, null, null, null);
            if (metaCursor != null)
            {
                try
                {
                    if (metaCursor.MoveToFirst())
                    {
                        path = metaCursor.GetString(0);
                    }
                }
                finally
                {
                    metaCursor.Close();
                }

            }
            return path;
        }
    }
}