using PM2P1_T4.Model;
using PM2P1_T4.View;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace PM2P1_T4
{
    public partial class MainPage : ContentPage
    {
        public MainPage()
        {
            InitializeComponent();
        }


        protected async override void OnAppearing()
        {
            base.OnAppearing();

            try
            {
                listImage.ItemsSource = await App.DBase.getListImagen();
            }
            catch (Exception)
            {

            }
        }

        private async void toolMenu1_Clicked(object sender, EventArgs e)
        {
            await Navigation.PushAsync(new AddPage());
        }

        private async void toolMenu2_Clicked(object sender, EventArgs e)
        {
            await Navigation.PushAsync(new LocalPage());
        }

        private async void listImage_ItemTapped(object sender, ItemTappedEventArgs e)
        {
            var selected = e.Item as Imagen;

            //await DisplayAlert("","Elemento seleccionado: " + Emple.nombre + " Fecha: "+ Emple.fechaIngreso, "OK");

            ImagePage viewEmple = new ImagePage();
            viewEmple.BindingContext = selected;

            await Navigation.PushAsync(viewEmple);
        }
    }
}