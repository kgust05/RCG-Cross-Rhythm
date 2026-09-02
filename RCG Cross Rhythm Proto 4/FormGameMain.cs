using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Diagnostics;
using System.Media;

namespace RCG_Cross_Rhythm_Proto_4
{
    //the main game/menu class/form
    //handles input/output and time-related properties
    public partial class FormGameMain : Form
    {
        private Stopwatch barStopwatch = new Stopwatch();
        private Button[] notes = new Button[16];
        private bool antiSpam = false;
        private int keyValueCache = 0;
        private string[] possibleKeyValues = new string[] { "W", "A", "S", "D" };
        private SoundPlayer countInNormal = new SoundPlayer(Properties.Resources.countInNormal);
        private SoundPlayer countInHard = new SoundPlayer(Properties.Resources.countInHard);
        private SoundPlayer gameTrack = new SoundPlayer(Properties.Resources.gameTrack);

        public FormGameMain()
        {
            InitializeComponent();
        }

        //prepares aspects of the program that has not already been defined above
        //used similarly to a constructor
        private void FormGameMain_Load(object sender, EventArgs e)
        {
            for (int i = 0; i < 16; i++)
            {
                notes[i] = Controls[$"note{i + 1}"] as Button;
            }

            foreach (Button note in notes)
            {
                note.KeyDown += NoteKeyDown;
                note.KeyUp += NoteKeyUp;
                note.MouseEnter += NoteMouseEnter;

                //acts as an extra failsafe for key inputs on notes if ActiveControl decides to be weird
                note.Tag = false;
            }

            countInNormal.Load();
            countInHard.Load();
            gameTrack.Load();
            PreloadSounds();
        }

        //used to allow antiSpam functions to update on a key press when no note button is focused
        private void FormGameMain_KeyDown(object sender, KeyEventArgs e)
        {
            keyValueCache = e.KeyValue;
            antiSpam = true;
        }

        //disables antiSpam functions on a key release when no note button is focused
        private void FormGameMain_KeyUp(object sender, KeyEventArgs e)
        {
            if (e.KeyValue == keyValueCache)
            {
                antiSpam = false;
            }
        }

        //makes sure the form is focused when the mouse is not over a note button
        //this allows the related KeyDown/Up event handlers to trigger
        private void FormGameMain_MouseEnter(object sender, EventArgs e)
        {
            ActiveControl = null;
        }

        //play button that starts a new loop
        //includes note generation, changing to game screen, difficulty attribute changes and preloading sounds to play in time
        private void btnPlay_Click(object sender, EventArgs e)
        {
            PreloadSounds();
            Generation.Reset();
            Visuals.Reset();
            Game.Reset();
            tmMetronome.Interval = Game.GetInterval();
            tmCountIn.Interval = (2000 * Game.GetCountInAmount()) - 60;

            Visuals.UpdateHUD();
            Visuals.GamePlayState();
            Generation.OffsetNotes();

            for (int i = 0; i < Game.GetSeqAmount(); i++)
            {
                Generation.Generate();
            }

            tmPreloadBuffer.Start();
        }

        //switches the difficulty to normal
        private void btnNormal_Click(object sender, EventArgs e)
        {
            Game.SetDifficultyProperties(8, 500, 2, 4, "Normal");            
            Visuals.GameDiffSelect(true, "Standard gameplay.");
        }

        //switches the difficulty to hard
        private void btnHard_Click(object sender, EventArgs e)
        {
            Game.SetDifficultyProperties(16, 250, 1, 2, "Hard");
            Visuals.GameDiffSelect(false, "A test of rhythm.");
        }

        //closes the game
        private void btnExit_Click(object sender, EventArgs e)
        {
            Close();
        }

        //starts the brief display of the score display for individual note hits
        //if a score display is already active, also resets the relevant timer
        private void lblHitDisplay_LocationChanged(object sender, EventArgs e)
        {
            lblHitDisplay.Visible = true;
            tmHitDisplayLife.Stop();
            tmHitDisplayLife.Start();
        }

        //general note event handler for key presses
        //carries out antiSpam, specific key input and timing checks to return into Game class for score calculation
        private void NoteKeyDown(object sender, KeyEventArgs e)
        {
            Button note = sender as Button;

            if ((ActiveControl == note) && ((bool)note.Tag == true))
            {
                if (!antiSpam || (e.KeyValue != keyValueCache))
                {
                    foreach (string s in possibleKeyValues)
                    {
                        Enum.TryParse(s, out Keys possibleInput);

                        if (possibleInput == e.KeyCode)
                        {
                            for (int i = 0; i < 16; i++)
                            {
                                if (note == notes[i])
                                {
                                    Game.Calculate(i, (int)barStopwatch.ElapsedMilliseconds % (Game.GetInterval() * 16), s);
                                    Visuals.DisplayNotes();
                                    Visuals.UpdateHUD();
                                    GameOverProcedure();

                                    break;
                                }
                            }

                            break;
                        }
                    }
                }
            }

            keyValueCache = e.KeyValue;
            antiSpam = true;
        }

        //disables antiSpam functions when a note button is in focus
        private void NoteKeyUp(object sender, KeyEventArgs e)
        {
            if (e.KeyValue == keyValueCache)
            {
                antiSpam = false;
            }
        }

        //makes the relevant note button (sender) the active control so that Key event handlers trigger
        //also makes the note Tag true as an extra failsafe for correct key input registering
        private void NoteMouseEnter(object sender, EventArgs e)
        {
            Button note = sender as Button;

            ActiveControl = note;
            note.Tag = true;
        }

        //makes the note Tag false as an extra failsafe for correct key input registering
        private void NoteMouseLeave(object sender, EventArgs e)
        {
            Button note = sender as Button;

            note.Tag = false;
        }

        //pre-emptively plays all sounds abruptly so that the sounds play accurately in time when needed
        private void PreloadSounds()
        {
            countInNormal.Play();
            countInHard.Play();
            gameTrack.Play();
            tmPreloadTime.Start();
        }

        //plays correct countIn sound depending on difficulty
        private void PlayCountIn()
        {
            if (!btnNormal.Enabled)
            {
                countInNormal.Play();
            }
            else if (!btnHard.Enabled)
            {
                countInHard.Play();
            }
        }

        //checks if player has lost the game and stops the game accordingly
        //manipulates timing-related properties and visuals
        private void GameOverProcedure()
        {
            if (Game.CheckIfGameOver())
            {
                gameTrack.Stop();
                tmMetronome.Stop();
                tmHitDisplayLife.Stop();
                barStopwatch.Reset();
                Visuals.GameEndState("Game Over");
            }
        }

        //manages non-input related game display and score calculation
        //also checks if player has won the game and stops game accordingly
        private void tmMetronome_Tick(object sender, EventArgs e)
        {
            if (!barStopwatch.IsRunning)
            {
                PlayCountIn();
                tmCountIn.Start();
                barStopwatch.Start();
            }

            Visuals.AddNextNote();
            Game.UpdateLifetime();
            Visuals.DisplayNotes();
            Game.RegisterLateMiss();
            Visuals.UpdateHUD();

            if (Game.CheckIfFinished())
            {
                tmMetronome.Stop();
                barStopwatch.Reset();
                Visuals.GameEndState("Game Cleared");
            }

            GameOverProcedure();
        }

        //hides lblHitDisplay after a certain interval of being visible
        private void tmHitDisplayLife_Tick(object sender, EventArgs e)
        {
            lblHitDisplay.Visible = false;
            tmHitDisplayLife.Stop();
        }

        //plays the gameTrack after countIn has finished
        private void tmCountIn_Tick(object sender, EventArgs e)
        {
            gameTrack.Play();
            tmCountIn.Stop();
        }

        //stops playing the sounds for PreloadSounds
        private void tmPreloadTime_Tick(object sender, EventArgs e)
        {
            countInNormal.Stop();
            countInHard.Stop();
            gameTrack.Stop();
            tmPreloadTime.Stop();
        }

        //starts the game after it is prepped when btnPlay is clicked
        private void tmPreloadBuffer_Tick(object sender, EventArgs e)
        {
            tmMetronome.Start();
            tmPreloadBuffer.Stop();
        }
    }
}
