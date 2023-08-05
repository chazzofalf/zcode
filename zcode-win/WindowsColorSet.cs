using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using zcode_api_std;

namespace zcode_win
{
    internal class WindowsColorSet : IColorSet
    {
        private IColor _turquoise;
        private IColor _black;
        public IColor Turquoise => _turquoise = _turquoise != null ? _turquoise : ((Func<IColor>)(() => {
            return new WindowColor(System.Drawing.Color.FromArgb(64, 244, 208));
        }))(); //R: 64, G: 224, B: 208

        public IColor Black => _black = _black != null ? _black : ((Func<IColor>)(() => {
            return new WindowColor(System.Drawing.Color.FromArgb(0, 0, 0));
        }))(); //R: 64, G: 224, B: 208
    }
}
