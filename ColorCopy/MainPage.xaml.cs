using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Media.Animation;
using Windows.UI.Xaml.Navigation;

namespace ColorCopy
{
    public sealed partial class MainPage : Page
    {
        const int NUMBER_OF_COLORS = 4;
        const int COLOR_1 = 1;
        const int COLOR_2 = 2;
        const int COLOR_3 = 3;
        const int COLOR_4 = 4;
        const int MAX_SEQUENCE_COUNT = 100; // TODO: Make 100 for release version
        const String CLICK_TO_BEGIN = "Click to begin!";
        const String YOU_WIN = "YOU WIN!";

        int currentLengthOfSequence = 0;

        Random randomNumber;
        int[] correctColorSequence;
        int[] playerColorSequence;
        int correctColorSequenceIndex = 0;
        int playerColorSequenceIndex = 0;

        Boolean isPlayerTurn = true;
        Boolean isSequenceCorrect = false;
        Boolean isGameStarted = false;
        Boolean isDoingAnimation = false;

        long timeStart, timeEnd, timeDifference;

        public MainPage()
        {
            this.InitializeComponent();

            randomNumber = new Random();

            correctColorSequence = new int[MAX_SEQUENCE_COUNT];
            playerColorSequence = new int[MAX_SEQUENCE_COUNT];
            initializeColorSequence();

            textBlockRoundNumber.Opacity = 0;
        }

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
        }

        private void rectangle0_Tapped(object sender, TappedRoutedEventArgs e)
        {
            if (isPlayerTurn)
            {
                storyboardRectangle0Player.Begin();
                audioPiano12.Volume = 0.5;
                audioPiano12.Play();

                if (isGameStarted)
                {
                    playerColorSequence[playerColorSequenceIndex++] = COLOR_1;
                    isPlayerTurn = false;
                    continueGame(); 
                }
            }
        }
        private void rectangle1_Tapped(object sender, TappedRoutedEventArgs e)
        {
            if (isPlayerTurn)
            {
                storyboardRectangle1Player.Begin();
                audioPiano16.Volume = 0.5;
                audioPiano16.Play();

                if (isGameStarted)
                {
                    playerColorSequence[playerColorSequenceIndex++] = COLOR_2;
                    isPlayerTurn = false;
                    continueGame(); 
                }
            }
        }
        private void rectangle2_Tapped(object sender, TappedRoutedEventArgs e)
        {
            if (isPlayerTurn)
            {
                storyboardRectangle2Player.Begin();
                audioPiano19.Volume = 0.5;
                audioPiano19.Play();

                if (isGameStarted)
                {
                    playerColorSequence[playerColorSequenceIndex++] = COLOR_3;
                    isPlayerTurn = false;
                    continueGame(); 
                }
            }
        }
        private void rectangle3_Tapped(object sender, TappedRoutedEventArgs e)
        {
            if (isPlayerTurn)
            {
                storyboardRectangle3Player.Begin();
                audioPiano114.Volume = 0.5;
                audioPiano114.Play();

                if (isGameStarted)
                {
                    playerColorSequence[playerColorSequenceIndex++] = COLOR_4;
                    isPlayerTurn = false;
                    continueGame(); 
                }
            }
        }

        private void Tapped_TextBlockCenter(object sender, TappedRoutedEventArgs e)
        {
            if (textBlockCenter.Text.Equals(CLICK_TO_BEGIN))
            {
                if (textBlockCenter.Opacity != 0)
                {
                    startGame(); 
                }
            }
            else if (textBlockCenter.Text.Equals(YOU_WIN))
            {
                endGame(CLICK_TO_BEGIN);
            }
            else
            {
                // Do nothing
            }
        }

        private void startGame()
        {
            currentLengthOfSequence = 0;
            //textBlockCenter.Text = "Round " + roundNumber;
            textBlockRoundNumber.Text = currentLengthOfSequence.ToString();
            textBlockRoundNumber.Foreground = new SolidColorBrush(Colors.White);
            textBlockRoundNumber.Opacity = 100;
            textBlockCenter.Opacity = 0;
            isGameStarted = true;
            playerColorSequenceIndex = 0;
            initializeColorSequence();
            isPlayerTurn = false;
            continueGame();
        }

        private void continueGame()
        {
            isSequenceCorrect = true;
            for (int i = 0; i < playerColorSequenceIndex; i++)
            {
                if (correctColorSequence[i] == playerColorSequence[i])
                {
                    // do nothing
                }
                else
                {
                    isSequenceCorrect = false;
                    break;
                }
            }

            if (isSequenceCorrect)
            {
                if (playerColorSequenceIndex >= currentLengthOfSequence)
                {
                    currentLengthOfSequence++;
                    if (currentLengthOfSequence < MAX_SEQUENCE_COUNT)
                    {
                        audioShinyDing.Volume = 0.5;
                        audioShinyDing.Play();
                        textBlockRoundNumber.Text = currentLengthOfSequence.ToString();
                    }
                    else // roundNumber == MAX_SEQUENCE_COUNT
                    {
                        audioApplause.Volume = 0.5;
                        audioApplause.Play();
                        endGame(YOU_WIN);
                    }
                }
                else // playerColorSequenceIndex < roundNumber
                {
                    //playerColorSequenceIndex++;
                    isPlayerTurn = true;
                }
            }
            else // isSquenceCorrect == false
            {
                endGame(CLICK_TO_BEGIN);
            }
        }

        private void endGame(String endGameMessage)
        {
            isGameStarted = false;
            textBlockCenter.Text = endGameMessage;
            textBlockCenter.Opacity = 100;
            textBlockRoundNumber.Foreground = new SolidColorBrush(Colors.Gray);
            textBlockRoundNumber.Opacity = 40;
            isPlayerTurn = true;
        }


        #region Helper Functions

        private void initializeColorSequence()
        {
            for (int i = 0; i < MAX_SEQUENCE_COUNT; i++)
            {
                correctColorSequence[i] = randomNumber.Next(1, NUMBER_OF_COLORS);
                playerColorSequence[i] = 0;
            }
        }

        private void waitMilliseconds(int waitTime) // Not used
        {
            timeStart = DateTime.Now.Ticks;

            long totalWaitTimeInTicks = waitTime * 10000;

            do
            {
                timeEnd = DateTime.Now.Ticks;
                timeDifference = timeEnd - timeStart;
            }
            while (timeDifference < totalWaitTimeInTicks);
        }

        #endregion

        #region Storyboard Functions

        private void storyboardRectangle0Complete(object sender, object e)
        {
                if (correctColorSequenceIndex < currentLengthOfSequence)
                {
                    doNextAnimation();
                }
                else
                {
                    isDoingAnimation = false;
                    isPlayerTurn = true;
                    correctColorSequenceIndex = 0;
                    playerColorSequenceIndex = 0;
                } 
        }
        private void storyboardRectangle1Complete(object sender, object e)
        {
                if (correctColorSequenceIndex < currentLengthOfSequence)
                {
                    doNextAnimation();
                }
                else
                {
                    isDoingAnimation = false;
                    isPlayerTurn = true;
                    correctColorSequenceIndex = 0;
                    playerColorSequenceIndex = 0;
                } 
        }
        private void storyboardRectangle2Complete(object sender, object e)
        {
                if (correctColorSequenceIndex < currentLengthOfSequence)
                {
                    doNextAnimation();
                }
                else
                {
                    isDoingAnimation = false;
                    isPlayerTurn = true;
                    correctColorSequenceIndex = 0;
                    playerColorSequenceIndex = 0;
                } 
        }
        private void storyboardRectangle3Complete(object sender, object e)
        {
                if (correctColorSequenceIndex < currentLengthOfSequence)
                {
                    doNextAnimation();
                }
                else
                {
                    isDoingAnimation = false;
                    isPlayerTurn = true;
                    correctColorSequenceIndex = 0;
                    playerColorSequenceIndex = 0;
                } 
        }

        private void doNextAnimation()
        {
                if (correctColorSequence[correctColorSequenceIndex] == 1)
                {
                    audioPiano12.Volume = 0.5;
                    audioPiano12.Play();
                    storyboardRectangle0.Begin();
                    isDoingAnimation = true;
                }
                else if (correctColorSequence[correctColorSequenceIndex] == 2)
                {
                    audioPiano16.Volume = 0.5;
                    audioPiano16.Play();
                    storyboardRectangle1.Begin();
                    isDoingAnimation = true;
                }
                else if (correctColorSequence[correctColorSequenceIndex] == 3)
                {
                    storyboardRectangle2.Begin();
                    audioPiano19.Volume = 0.5;
                    audioPiano19.Play();
                    isDoingAnimation = true;
                }
                else if (correctColorSequence[correctColorSequenceIndex] == 4)
                {
                    storyboardRectangle3.Begin();
                    audioPiano114.Volume = 0.5;
                    audioPiano114.Play();
                    isDoingAnimation = true;
                }
                correctColorSequenceIndex++;
        }

        #endregion

        private void audioShinyDing_MediaEnded(object sender, RoutedEventArgs e)
        {
            if (isGameStarted)
            {
                doNextAnimation(); 
            }
        }

        private void audioApplause_MediaEnded(object sender, RoutedEventArgs e)
        {
        }

    }
}
