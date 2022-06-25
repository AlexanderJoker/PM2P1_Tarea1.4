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

        MediaFile FileFoto = null;

        public AddPage()
        {
            InitializeComponent();
        }

        private async void btnTomarFoto_Clicked(object sender, EventArgs e)
        {
            try
            {
                FileFoto = await CrossMedia.Current.TakePhotoAsync(new Plugin.Media.Abstractions.StoreCameraMediaOptions
                {
                    SaveToAlbum = true,
                    Name = Nombre.Text,
                    Directory = "MisFotos",
                    CompressionQuality = 20,
                    CustomPhotoSize = 20
                });

                await DisplayAlert("Direcctorio", FileFoto.Path, "OK");
            }
            catch (Exception ex)
            {
                await DisplayAlert("Error", ex.Message, "OK");
            }

            

            if (FileFoto != null)
            {
                Foto.Source = ImageSource.FromStream(() => {

                    return FileFoto.GetStream();
                });
            }
        }

        private async void btnGuardar_Clicked(object sender, EventArgs e)
        {
            if (FileFoto == null)
            {
                await DisplayAlert("Error", "No se a tomado una fotografia", "OK");
                return;
            }

            if (string.IsNullOrEmpty(Nombre.Text.Trim()) || string.IsNullOrEmpty(Descripcion.Text.Trim()))
            {
                await DisplayAlert("Error", "Debe llenar todos los campos", "OK");
                return;
            }

            Imagen imagen = new Imagen()
            {
                nombre = Nombre.Text,
                descripcion = Descripcion.Text,
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
            if (FileFoto != null)
            {
                using (MemoryStream memory = new MemoryStream())
                {
                    Stream stream = FileFoto.GetStream();

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
            Nombre.Text = "";
            Descripcion.Text = "";
            Foto.Source = null;
        }

    }

}