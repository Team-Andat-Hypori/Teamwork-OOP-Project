using System;
using System.Windows.Forms;
using TeamAndatHypori.CoreLogic;
using TeamAndatHypori.Exceptions;

namespace TeamAndatHypori
{
    class GameMain
    {
        static void Main(string[] args)
        {
            try
            {
                using ( var engine = new Engine())
                {
                    engine.Run();
                }
            }
            catch (ResourcesNotFoundException re)
            {
                MessageBox.Show(re.Message);
            }
            catch (Exception e)
            {
                MessageBox.Show(e.Message);
            }
        }
    }
}
