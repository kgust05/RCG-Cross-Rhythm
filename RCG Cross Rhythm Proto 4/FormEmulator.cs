using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace RCG_Cross_Rhythm_Proto_4
{
    //static class used as a means of accessing and manipulating controls in FormGameMain from other classes
    public static class FormEmulator
    {
        private static Form FormGameMain;
        private static Button[] notes;
        private static Label lblScore;
        private static Label lblCombo;
        private static Label lblHP;
        private static ProgressBar pbHealthBar;
        private static Label lblHitDisplay;
        private static Button btnPlay;
        private static Button btnNormal;
        private static Button btnHard;
        private static Button btnExit;
        private static Label lblDiffComment;
        private static Label lblEndState;
        private static Label lblFinalScore;
        private static Label lblMaxCombo;
        private static Label lblMode;
        private static Label lblFinalScoreDisplay;
        private static Label lblMaxComboDisplay;
        private static Label lblModeDisplay;
        private static Label lblTitle;
        private static Label lblInstructions;

        static FormEmulator()
        {
            FormGameMain = Application.OpenForms["FormGameMain"];
            InitialiseNotes();
            lblScore = Application.OpenForms["FormGameMain"].Controls["lblScore"] as Label;
            lblCombo = Application.OpenForms["FormGameMain"].Controls["lblMultiplier"] as Label;
            lblHP = Application.OpenForms["FormGameMain"].Controls["lblHP"] as Label;
            pbHealthBar = Application.OpenForms["FormGameMain"].Controls["pbHealthBar"] as ProgressBar;
            lblHitDisplay = Application.OpenForms["FormGameMain"].Controls["lblHitDisplay"] as Label;
            btnPlay = Application.OpenForms["FormGameMain"].Controls["btnPlay"] as Button;
            btnNormal = Application.OpenForms["FormGameMain"].Controls["btnNormal"] as Button;
            btnHard = Application.OpenForms["FormGameMain"].Controls["btnHard"] as Button;
            btnExit = Application.OpenForms["FormGameMain"].Controls["btnExit"] as Button;
            lblDiffComment = Application.OpenForms["FormGameMain"].Controls["lblDiffComment"] as Label;
            lblEndState = Application.OpenForms["FormGameMain"].Controls["lblEndState"] as Label;
            lblFinalScore = Application.OpenForms["FormGameMain"].Controls["lblFinalScore"] as Label;
            lblMaxCombo = Application.OpenForms["FormGameMain"].Controls["lblMaxCombo"] as Label;
            lblMode = Application.OpenForms["FormGameMain"].Controls["lblMode"] as Label;
            lblFinalScoreDisplay = Application.OpenForms["FormGameMain"].Controls["lblFinalScoreDisplay"] as Label;
            lblMaxComboDisplay = Application.OpenForms["FormGameMain"].Controls["lblMaxComboDisplay"] as Label;
            lblModeDisplay = Application.OpenForms["FormGameMain"].Controls["lblModeDisplay"] as Label;
            lblTitle = Application.OpenForms["FormGameMain"].Controls["lblTitle"] as Label;
            lblInstructions = Application.OpenForms["FormGameMain"].Controls["lblInstructions"] as Label;
        }

        //method used to make initialising the note buttons quicker
        private static void InitialiseNotes()
        {
            notes = new Button[16];

            for (int i = 0; i < 16; i++)
            {
                notes[i] = Application.OpenForms["FormGameMain"].Controls[$"note{i + 1}"] as Button;
            }
        }

        //getters here
        public static Form GetFormGameMain()
        {
            return FormGameMain;
        }

        public static Button[] GetNotes()
        {
            return notes;
        }

        public static Label GetLblScore()
        {
            return lblScore;
        }

        public static Label GetLblCombo()
        {
            return lblCombo;
        }

        public static Label GetLblHP()
        {
            return lblHP;
        }

        public static ProgressBar GetPbHealthBar()
        {
            return pbHealthBar;
        }

        public static Label GetLblHitDisplay()
        {
            return lblHitDisplay;
        }

        public static Button GetBtnPlay()
        {
            return btnPlay;
        }

        public static Button GetBtnNormal()
        {
            return btnNormal;
        }

        public static Button GetBtnHard()
        {
            return btnHard;
        }

        public static Button GetBtnExit()
        {
            return btnExit;
        }

        public static Label GetLblDiffComment()
        {
            return lblDiffComment;
        }

        public static Label GetLblEndState()
        {
            return lblEndState;
        }

        public static Label GetLblFinalScore()
        {
            return lblFinalScore;
        }

        public static Label GetLblMaxCombo()
        {
            return lblMaxCombo;
        }

        public static Label GetLblMode()
        {
            return lblMode;
        }

        public static Label GetLblFinalScoreDisplay()
        {
            return lblFinalScoreDisplay;
        }

        public static Label GetLblMaxComboDisplay()
        {
            return lblMaxComboDisplay;
        }

        public static Label GetLblModeDisplay()
        {
            return lblModeDisplay;
        }

        public static Label GetLblTitle()
        {
            return lblTitle;
        }

        public static Label GetLblInstructions()
        {
            return lblInstructions;
        }
    }
}
