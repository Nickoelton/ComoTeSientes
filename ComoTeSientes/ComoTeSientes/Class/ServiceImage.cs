using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Plugin.Media;
using Plugin.Media.Abstractions;

namespace ComoTeSientes.Class // Este es el nombre de tu aplicacion.Class
{
    class ServiceImage
    {
        public static async Task<MediaFile> TakePicture(bool useCam)
        {
            await CrossMedia.Current.Initialize();

            if (useCam)  // Preguntamos si usamos la cámara
            {
                if (!CrossMedia.Current.IsCameraAvailable || !CrossMedia.Current.IsTakePhotoSupported)
                {
                    return null;
                }
            }

            var file = useCam
            ? await CrossMedia.Current.TakePhotoAsync(new StoreCameraMediaOptions
            {
                Directory = "Cognitive",  
                Name = "SC_Test.jpg"        // El nombre temporal como se guardará la foto
            })
            : await CrossMedia.Current.PickPhotoAsync();

            return file;
        }
    }
}
