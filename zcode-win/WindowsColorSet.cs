using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using zcode_api_std;
using zcode_common_std;
namespace zcode_win
{
    internal class WindowsColorSet : IColorSet
    {
        private enum BadColorsEnum
        {
            Turquoise,
            Black
        }
        private Util.LazyVariable<Dictionary<BadColorsEnum, IColor>> _bad = Util.LazyVariable<Dictionary<BadColorsEnum, IColor>>.Create(() =>
        {
            var baddict = new Dictionary<BadColorsEnum, IColor>();
            baddict[BadColorsEnum.Turquoise] = new WindowColor(System.Drawing.Color.FromArgb(64, 244, 208));
            baddict[BadColorsEnum.Black] = new WindowColor(System.Drawing.Color.FromArgb(0, 0, 0));
            return baddict;
        });
        private IColor _turquoise;
        private IColor _black;
        public IColor Turquoise => _bad.Get[BadColorsEnum.Turquoise]; //R: 64, G: 224, B: 208

        public IColor Black => _bad.Get[BadColorsEnum.Black]; //R: 0, G: 0, B: 0
    }
}
