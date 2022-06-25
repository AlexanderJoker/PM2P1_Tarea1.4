using Plugin.Media;
using Plugin.Media.Abstractions;
using PM2P1_T4.Model;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace PM2P1_T4.View
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class AddPage : ContentPage
    {

        MediaFile FileImage = null;

        public AddPage()
        {
            InitializeComponent();
        }

        private async void btnTomarFoto_Clicked(object sender, EventArgs e)
        {
            try
            {
                FileImage = await CrossMedia.Current.TakePhotoAsync(new Plugin.Media.Abstractions.StoreCameraMediaOptions
                {
                    SaveToAlbum = true,
                    Name = Nombres.Text,
                    Directory = "MisFotos",
                    CompressionQuality = 20,
                    CustomPhotoSize = 20
                });

                await DisplayAlert("Direcctorio", FileImage.Path, "OK");
            }
            catch (Exception ex)
            {
                await DisplayAlert("Error", ex.Message, "OK");
            }

            

            if (FileImage != null)
            {
                Foto.Source = ImageSource.FromStream(() => {

                    return FileImage.GetStream();
                });
            }
        }

        private async void btnGuardar_Clicked(object sender, EventArgs e)
        {
            if (FileImage == null)
            {
                await DisplayAlert("Error", "No se a tomado una fotografia", "OK");
                return;
            }

            if (string.IsNullOrEmpty(Nombres.Text.Trim()) || string.IsNullOrEmpty(Descripciones.Text.Trim()))
            {
                await DisplayAlert("Error", "Debe llenar todos los campos", "OK");
                return;
            }

            Imagen imagen = new Imagen()
            {
                nombre = Nombres.Text,
                descripcion = Descripciones.Text,
                foto = ConvertImageToByteArray()

            };


            var result = await App.DBase.insertUpdateImagen(imagen);

            if (result > 0)
            {
                await DisplayAlert("Imagen agregada", "Aviso", "OK");
            }
            else
            {
                await DisplayAlert("Ha ocurrido un error", "Aviso", "OK");
            }

            limpiar();

        }

        private Byte[] ConvertImageToByteArray()
        {
            if (FileImage != null)
            {
                using (MemoryStream memory = new MemoryStream())
                {
                    Stream stream = FileImage.GetStream();

                    stream.CopyTo(memory);

                    return memory.ToArray();
                }
            }

            return null;
        }


        public async void RetriveImageFromLocation(string location)
        {
            var memoryStream = new MemoryStream();

            using (var source = System.IO.File.OpenRead(location))
            {
                await source.CopyToAsync(memoryStream);
            }

            Foto.Source = ImageSource.FromStream(() => {
                return new MemoryStream(memoryStream.GetBuffer());
            });
        }


        private void limpiar()
        {
            Nombres.Text = "";
            Descripciones.Text = "";
            Foto.Source = null;
        }

    }

}