using CommunityToolkit.Maui.Storage;
using Microsoft.Maui.Hosting;
using System.IO;
using System.Threading;
using zcode_base;
using zcode_rsrcs;
using zcode_skia;

namespace zcode_app_2
{
    public partial class MainPage : ContentPage
    {
        int count = 0;
        ZethanaCode coder = null;
        public MainPage()
        {
            InitializeComponent();
            var fs = typeof(Resources).Assembly.GetManifestResourceStream(zcode_rsrcs.Resources.GoodDemoResourceName);
            var ts = new System.IO.StreamReader(fs);
            var msg = ts.ReadToEnd();
            RegText.Text = msg;
            _ = UpdateImageToText();
        }
        private async Task UpdateImageToText()
        {
            await Task.Yield();
            SkiaGraphicsSystem sgs = new SkiaGraphicsSystem();
            if (coder == null)
            {
                coder = await ZethanaCode.InitAsync(sgs);
            }
            var bm = await coder.FromTextAsync(RegText.Text);
            var png = bm.PNGData;

            AtTextImg.Source = ImageSource.FromStream(() => png);
        }
        
        private async Task LoadImage()
        {
            await Task.Yield();
            var po = global::Microsoft.Maui.Storage.PickOptions.Default;
            var x = await global::Microsoft.Maui.Storage.FilePicker.PickAsync(po);
            if (x != null)
            {
                var path = x.FullPath;
                RegText.Text = coder.FromBitmap(path);
                var bm = await coder.FromTextAsync(RegText.Text);
                var png = bm.PNGData;

                AtTextImg.Source = ImageSource.FromStream(() => png);
            }
        }
        private async Task SaveImage()
        {
            await Task.Yield();
            await Task.Yield();
            SkiaGraphicsSystem sgs = new SkiaGraphicsSystem();
            if (coder == null)
            {
                coder = await ZethanaCode.InitAsync(sgs);
            }
            CancellationTokenSource cts = new CancellationTokenSource();
            CancellationToken token = cts.Token;
            var bm = await coder.FromTextAsync(RegText.Text);
            var png = bm.PNGData;
            var pick = await FileSaver.Default.SaveAsync("output.png", png, token);
        }

        private void Refresh_Clicked(object sender, EventArgs e)
        {
            _ = UpdateImageToText();
        }

        private void Load_Clicked(object sender, EventArgs e)
        {
            _ = LoadImage();
        }

        private void Save_Clicked(object sender, EventArgs e)
        {
            _ = SaveImage();
        }

        private void View_Clicked(object sender, EventArgs e)
        {

        }

        private void Share_Clicked(object sender, EventArgs e)
        {

        }
    }
}