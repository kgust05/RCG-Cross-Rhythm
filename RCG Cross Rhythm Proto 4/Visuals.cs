using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;

namespace RCG_Cross_Rhythm_Proto_4
{
    //used to control how visible controls are displayed in FormGameMain
    public static class Visuals
    {
        private static CustomQueue<Note> activeQueue;
        private static OverflowArray<Note> displayedNotes;

        static Visuals()
        {
            activeQueue = new CustomQueue<Note>();
            displayedNotes = new OverflowArray<Note>(5);
        }

        //adds notes to queue of notes that will be displayed
        public static void EnqueueNote(Note note)
        {
            activeQueue.Enqueue(note);
        }

        //adds a note to queue of notes that are being displayed
        public static void AddNextNote()
        {
            displayedNotes.Enqueue(activeQueue.Dequeue());
        }

        //displays appropriate controls when a game is in session
        public static void GamePlayState()
        {
            FormEmulator.GetLblTitle().Visible
                = FormEmulator.GetLblInstructions().Visible
                = FormEmulator.GetLblEndState().Visible
                = FormEmulator.GetLblFinalScore().Visible
                = FormEmulator.GetLblMaxCombo().Visible
                = FormEmulator.GetLblMode().Visible
                = FormEmulator.GetLblFinalScoreDisplay().Visible
                = FormEmulator.GetLblMaxComboDisplay().Visible
                = FormEmulator.GetLblModeDisplay().Visible
                = FormEmulator.GetLblDiffComment().Visible
                = FormEmulator.GetBtnPlay().Visible
                = FormEmulator.GetBtnNormal().Visible
                = FormEmulator.GetBtnHard().Visible
                = FormEmulator.GetBtnPlay().Visible
                = FormEmulator.GetBtnExit().Visible
                = false;

            FormEmulator.GetLblScore().Visible
                = FormEmulator.GetLblCombo().Visible
                = FormEmulator.GetLblHP().Visible
                = FormEmulator.GetPbHealthBar().Visible
                = true;
        }

        //displays appropriate controls when the game ends, with input to determine game over/clear
        public static void GameEndState(string endState)
        {
            for (int i = 0; i < 16; i++)
            {
                FormEmulator.GetNotes()[i].Visible = false;
            }

            FormEmulator.GetLblScore().Visible
                = FormEmulator.GetLblCombo().Visible
                = FormEmulator.GetLblHP().Visible
                = FormEmulator.GetPbHealthBar().Visible
                = FormEmulator.GetLblHitDisplay().Visible
                = false;

            FormEmulator.GetLblEndState().Text = endState;
            FormEmulator.GetLblFinalScoreDisplay().Text = Game.GetScore().ToString();
            FormEmulator.GetLblMaxComboDisplay().Text = $"x{Game.GetMaxCombo()}";
            FormEmulator.GetLblModeDisplay().Text = Game.GetMode();

            FormEmulator.GetLblTitle().Visible
                = FormEmulator.GetLblInstructions().Visible
                = FormEmulator.GetLblEndState().Visible
                = FormEmulator.GetLblFinalScore().Visible
                = FormEmulator.GetLblMaxCombo().Visible
                = FormEmulator.GetLblMode().Visible
                = FormEmulator.GetLblFinalScoreDisplay().Visible
                = FormEmulator.GetLblMaxComboDisplay().Visible
                = FormEmulator.GetLblModeDisplay().Visible
                = FormEmulator.GetLblDiffComment().Visible
                = FormEmulator.GetBtnPlay().Visible
                = FormEmulator.GetBtnNormal().Visible
                = FormEmulator.GetBtnHard().Visible
                = FormEmulator.GetBtnPlay().Visible
                = FormEmulator.GetBtnExit().Visible
                = true;
        }

        //controls which difficulty buttons you can click, depending on which one is currently selected
        public static void GameDiffSelect(bool isNormalDiff, string diffText)
        {
            FormEmulator.GetBtnNormal().Enabled = !isNormalDiff;
            FormEmulator.GetBtnHard().Enabled = isNormalDiff;
            FormEmulator.GetLblDiffComment().Text = diffText;
        }

        //displays the notes from displayedNotes queue
        //also manages the hiding of missed notes
        public static void DisplayNotes()
        {
            for (int i = 0; i < 16; i++)
            {
                //this is used to check if the current note is being displayed
                bool isActive = false;

                foreach (Note note in displayedNotes.GetList())
                {
                    if (note != default)
                    {
                        if (i == note.GetPosition())
                        {
                            isActive = true;
                            DisplayDirectionImage(note.GetToPress(), i);
                            FormEmulator.GetNotes()[i].Left = note.GetCoords()[0];
                            FormEmulator.GetNotes()[i].Top = note.GetCoords()[1];

                            if (note.GetClicked())
                            {
                                FormEmulator.GetNotes()[i].Visible = false;
                            }
                            else
                            {
                                FormEmulator.GetNotes()[i].Visible = note.GetEnabledStat();
                            }

                            break;
                        }
                    }
                }

                if (!isActive)
                {
                    FormEmulator.GetNotes()[i].Visible = false;
                }
            }

            DisplayTiming();
        }

        //displays different colours on the note buttons to indicate when you should press
        //for reference: greys = not ready yet, gold = on the early side, green = on the late side (PRESS NOW)
        //players should aim to press notes as close to between gold and green as possible
        private static void DisplayTiming()
        {
            foreach (Note note in displayedNotes.GetList())
            {
                if (note != default)
                {
                    try
                    {
                        switch (note.GetLifetime())
                        {
                            case 1:
                                FormEmulator.GetNotes()[note.GetPosition()].BackColor = Color.DimGray;

                                break;

                            case 2:
                                FormEmulator.GetNotes()[note.GetPosition()].BackColor = Color.Gray;

                                break;

                            case 3:
                                FormEmulator.GetNotes()[note.GetPosition()].BackColor = Color.DarkGray;

                                break;

                            case 4:
                                FormEmulator.GetNotes()[note.GetPosition()].BackColor = Color.Gold;

                                break;

                            case 5:
                                FormEmulator.GetNotes()[note.GetPosition()].BackColor = Color.Green;

                                break;
                        }
                    }
                    catch (IndexOutOfRangeException) { }
                }
            }
        }

        //displays different images based on which direction you need to press
        private static void DisplayDirectionImage(string toPress, int pos)
        {
            switch (toPress)
            {
                case "W":
                    FormEmulator.GetNotes()[pos].BackgroundImage = Properties.Resources.up;

                    break;

                case "A":
                    FormEmulator.GetNotes()[pos].BackgroundImage = Properties.Resources.left;

                    break;

                case "S":
                    FormEmulator.GetNotes()[pos].BackgroundImage = Properties.Resources.down;

                    break;

                case "D":
                    FormEmulator.GetNotes()[pos].BackgroundImage = Properties.Resources.right;

                    break;
            }
        }

        //updates score and health elements
        public static void UpdateHUD()
        {
            FormEmulator.GetLblScore().Text = Game.GetScore().ToString();
            FormEmulator.GetLblHP().Text = $"{Game.GetHealth()}HP";
            FormEmulator.GetLblCombo().Text = $"x{Game.GetCombo()}";
            FormEmulator.GetPbHealthBar().Value = Game.GetHealth();
        }

        //manages the displaying of individual hit scores
        public static void DisplayHitDisplay(Note note, string score)
        {
            FormEmulator.GetLblHitDisplay().Text = score;
            FormEmulator.GetLblHitDisplay().Left = FormEmulator.GetNotes()[note.GetPosition()].Left
                + (FormEmulator.GetNotes()[note.GetPosition()].Width / 2)
                - (FormEmulator.GetLblHitDisplay().Width / 2);
            FormEmulator.GetLblHitDisplay().Top = FormEmulator.GetNotes()[note.GetPosition()].Top
                + (FormEmulator.GetNotes()[note.GetPosition()].Height / 2)
                - (FormEmulator.GetLblHitDisplay().Height / 2);
        }

        //resets all relevant properties in the class to new objects ready for next game loop
        public static void Reset()
        {
            activeQueue = new CustomQueue<Note>();
            displayedNotes = new OverflowArray<Note>(5);
        }

        //getter for displayedNotes
        public static OverflowArray<Note> GetDisplayedNotes()
        {
            return displayedNotes;
        }
    }
}
