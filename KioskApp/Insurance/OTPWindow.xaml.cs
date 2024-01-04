using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace Insurance
{
    /// <summary>
    /// Interaction logic for OTPWindow.xaml
    /// </summary>
    public partial class OTPWindow : Window
    {

        string[] alphabet =
        {
            "1","2","3",
            "4","5","6",
            "7","8","9",
            "Reset","0","Del"
        };

        List<Border> removeKeyboardKey = new List<Border>();
        List<Border> chosenKeyboardKey = new List<Border>();
        private List<TextBox> textBoxes;

        public OTPWindow()
        {
            InitializeComponent();
            runVirtualKeyboad();

            textBoxes = new List<TextBox> { tboxInput, tboxInput2, tboxInput3, tboxInput4 };
        }

        //Numpad Design
        public void runVirtualKeyboad()
        {
            for (int i = 0; i < alphabet.Length; i++)
            {
                Border keyboardKey = new Border();
                keyboardKey.Width = 100;
                keyboardKey.Height = 60;
                keyboardKey.Margin = new Thickness(2, 3, 2, 3);
                keyboardKey.Background = Brushes.Gray;
                keyboardKey.CornerRadius = new CornerRadius(10);
                keyboardKey.Tag = alphabet[i];
                keyboardKey.Uid = i.ToString();
                keyboardKey.MouseLeftButtonDown += keyboardPressed;

                TextBlock teks = new TextBlock();
                teks.TextAlignment = TextAlignment.Center;
                teks.VerticalAlignment = VerticalAlignment.Center;
                teks.FontSize = 25;
                teks.Foreground = Brushes.White;
                teks.Text = alphabet[i];

                keyboardKey.Child = teks;
                keyboardRow1.Children.Add(keyboardKey);
                removeKeyboardKey.Add(keyboardKey);
                chosenKeyboardKey.Add(keyboardKey);

                if (removeKeyboardKey.Count > 3)
                {
                    keyboardRow1.Children.Remove(keyboardKey);
                    keyboardRow2.Children.Add(keyboardKey);

                    if (removeKeyboardKey.Count > 6)
                    {
                        keyboardRow1.Children.Remove(keyboardKey);
                        keyboardRow2.Children.Remove(keyboardKey);
                        keyboardRow3.Children.Add(keyboardKey);
                    }
                    if (removeKeyboardKey.Count > 9)
                    {
                        keyboardRow1.Children.Remove(keyboardKey);
                        keyboardRow2.Children.Remove(keyboardKey);
                        keyboardRow3.Children.Remove(keyboardKey);
                        keyboardRow4.Children.Add(keyboardKey);
                        if (removeKeyboardKey.Count == 10)
                        {
                            keyboardKey.Background = Brushes.Black;
                        }
                        if (removeKeyboardKey.Count == 12)
                        {
                            Image image = new Image();
                            image.Source = new BitmapImage(new Uri("Images/Delete.png", UriKind.RelativeOrAbsolute));

                            image.Width = 40;
                            image.Height = 40;

                            // Clear any existing content in keyboardKey
                            keyboardKey.Child = null;

                            // Add the image as a child to keyboardKey
                            keyboardKey.Child = image;
                            keyboardKey.Background = Brushes.Black;
                        }
                    }
                }
            }
        }




        private int currentTextBoxIndex = 0;


        private async void keyboardPressed(object sender, MouseButtonEventArgs e)
        {
            Border btn = sender as Border;
            string i = (string)btn.Tag;
            string index = (string)btn.Uid;

            TextBox currentTextBox = textBoxes[currentTextBoxIndex];


            if (i == "Del")
            {
                // Check if the text is now empty
                if (currentTextBox.Text.Length >= 0)
                {
                    currentTextBoxIndex = currentTextBoxIndex - 1;// do not skip to another textbox
                }
                switch (currentTextBoxIndex)
                {
                    case -1:
                        textBoxes[0].Text = "";
                        currentTextBoxIndex = -1;
                        break;
                    case 0:
                        textBoxes[1].Text = "";
                        currentTextBoxIndex--;
                        break;
                    case 1:
                        textBoxes[2].Text = "";
                        currentTextBoxIndex--;
                        break;
                    case 2:
                        textBoxes[3].Text = "";
                        currentTextBoxIndex--;
                        break;
                }
            }


            else if (i == "Reset")
            {
                foreach (TextBox textBox in textBoxes)
                {
                    textBox.Text = "";
                    currentTextBoxIndex = -1;
                }
            }

            else
            {
                // If the user pressed a regular button, append the character to the currentTextBox
                currentTextBox.Text += i;
                //Test.Text = currentTextBoxIndex.ToString();
            }

            // Increment the currentTextBoxIndex without resetting to 0 if it reaches the end
            currentTextBoxIndex = currentTextBoxIndex + 1;

            // Check if the index is now beyond the maximum
            if (currentTextBoxIndex >= textBoxes.Count)
            {
                // Handle the case where the index exceeds the maximum, for example, by setting it to the maximum index
                currentTextBoxIndex = textBoxes.Count - 1;
                if (currentTextBox.Text.Length > 1) // Delete latest character when exceeding
                {
                    currentTextBox.Text = currentTextBox.Text.Substring(0, currentTextBox.Text.Length - 1);
                }
            }

            //Numpad nha^p' nhay'
            chosenKeyboardKey[int.Parse(index)].Background = Brushes.White;
            await delay(100);
            chosenKeyboardKey[int.Parse(index)].Background = Brushes.Gray;
            chosenKeyboardKey[9].Background = Brushes.Black;
            chosenKeyboardKey[11].Background = Brushes.Black;
        }

        async Task delay(int num)
        {
            await Task.Delay(num);
        }




        private void tboxInput_TextChanged(object sender, TextChangedEventArgs e)
        {

        }
    }
}
