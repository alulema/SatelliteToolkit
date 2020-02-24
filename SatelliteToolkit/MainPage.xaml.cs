using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using SatelliteToolkit.Model;
using Syncfusion.XForms.Buttons;
using Xamarin.Forms;

namespace SatelliteToolkit
{
    [DesignTimeVisible(true)]
    public partial class MainPage : ContentPage
    {
        MenuCollectionViewModel vm;

        public MainPage()
        {
            InitializeComponent();
            hamburgerButton.ImageSource = (FileImageSource)ImageSource.FromFile("menu.png");

            vm = new MenuCollectionViewModel();
            this.listView.ItemsSource = vm.MenuCollection;

        }

        void hamburgerButton_Clicked(object sender, EventArgs e)
        {
            navigationDrawer.ToggleDrawer();
        }

        void Handle_ItemTapped(object sender, Syncfusion.ListView.XForms.ItemTappedEventArgs e)
        {
            var tempListView = sender as Syncfusion.ListView.XForms.SfListView;
            for (int i = 0; i < 3; i++)
            {
                var tempItem = (listView.ItemsSource as ObservableCollection<MenuCollectionModel>)[i];
                if ((e.ItemData as MenuCollectionModel) != tempItem)
                {
                    tempItem.FontColor = Color.FromHex("#8e8e92");
                }
            }

            var temp = e.ItemData as MenuCollectionModel;
            temp.FontColor = Color.FromHex("#007ad0");
            //navigationDrawer.ContentView = new Archive_Default(temp.MessageContent, (e.ItemData as MenuCollectionModel).MenuItem).Content;
            navigationDrawer.ToggleDrawer();

        }

        private void SfSwitch_OnStateChanged(object sender, SwitchStateChangedEventArgs e)
        {
            DisplayAlert("Message", "SUCCESS", "OK");
        }
    }
}
