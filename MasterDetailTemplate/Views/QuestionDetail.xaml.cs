
using System;
using System.IO;
using MasterDetailTemplate.Services;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace MasterDetailTemplate.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class QuestionDetail : ContentPage {
        // public IPhotoPickerService _iphotoPickerService;
        public QuestionDetail() {
            // _iphotoPickerService = iphotoPickerService;
            InitializeComponent();
        }

        // async void OnPickPhotoButtonClicked(object sender, EventArgs e)
        // {
        //     (sender as Button).IsEnabled = false;
        //     Stream stream = await _iphotoPickerService.GetImageStreamAsync();
        //     if (stream != null)
        //     {
        //         image.Source = ImageSource.FromStream(() => stream);
        //     }
        //
        //     (sender as Button).IsEnabled = true;
        // }

    }
}