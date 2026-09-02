using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RCG_Cross_Rhythm_Proto_4
{
    //record-style class that stores note button properties
    public class Note
    {
        private int x;
        private int y;
        private bool isEnabled;
        private int position;
        private string toPress;
        private bool clicked;
        private int lifetime;

        public Note(int x, int y, bool isEnabled, int position, string toPress)
        {
            this.x = x;
            this.y = y;
            this.isEnabled = isEnabled;
            this.position = position;
            this.toPress = toPress;
            clicked = false;
            lifetime = 0;
        }

        //getters here
        public int[] GetCoords()
        {
            return new int[] { x, y };
        }

        public bool GetEnabledStat()
        {
            return isEnabled;
        }

        public int GetPosition()
        {
            return position;
        }

        public string GetToPress()
        {
            return toPress;
        }

        public bool GetClicked()
        {
            return clicked;
        }

        public int GetLifetime()
        {
            return lifetime;
        }

        //setters here
        public void SetClicked(bool input)
        {
            clicked = input;
        }

        public void SetLifetime(int input)
        {
            lifetime = input;
        }
    }
}
