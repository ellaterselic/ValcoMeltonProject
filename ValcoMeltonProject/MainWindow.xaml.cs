using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using Xceed.Wpf.Toolkit;

namespace ValcoMeltonProject
{
    public partial class MainWindow : Window
    {

        public bool invalidCharEntry = false;
        public bool invalidWithMultipleDecimals = false;

        public decimal conversionAnswer;

        public int numKeyPresses = 0;
        public int check = 0;

        //handle input display for decimal box
        List<char> decBoxList = new List<char> {'0', '0', '0'};
        public char keyPress;
        public int decimalPlace = 2;
        public bool viewIsEnlarged = false;

        public MainWindow()
        {
            InitializeComponent();
            PopulateDropdownValues();
        }

        public bool ContainsNonNumbers(string input, bool allowDecimal)
        {
            string otherChars;

            if (allowDecimal)
            {
                otherChars = "`~!@#$%^&*()-_=+[{]}\\\\|;:'\",<>/?abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ\r\n";
            } else
            {
                otherChars = "`~!@#$%^&*()-_=+[{]}\\\\|;:'\",<.>/?abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ\r\n";
            }

            foreach (char c in input)
            {
                if (otherChars.Contains(c)) {
                    return true;
                }
            }
            return false;
        }

        private void InputEnteredOrChanged(object sender, TextChangedEventArgs e)
        {
            TextBox textBox = sender as TextBox;

            //check for unwanted characters

            bool containsNonNumbers = ContainsNonNumbers(textBox.Text, false);

            if (!containsNonNumbers)
            {
                //this is to reset the textbox color if it previously
                //had unwanted characters
                feedback.Text = " ";
                //SetBorderBlack(textBox);
                return;

            } else
            {
                feedback.Text = "Please enter integers only";
                //SetBorderRed(textBox);
            }
        }

        private void InputEnteredOrChangedCustomDecimal(object sender, TextChangedEventArgs e)
        {
            TextBox textBox = sender as TextBox;

            if (feedback != null)
            {
                bool containsNonNumbers = ContainsNonNumbers(textBox.Text, false);

                if (!containsNonNumbers)
                {
                    feedback.Text = " ";
                    invalidCharEntry = false;
   
                    if (textBox.Text != "")
                    {
                        int num = int.Parse(textBox.Text);

                        if (num > 5 || num < 1)
                        {
                            feedback.Text = "# of places after decimal can range from 1 - 5";
                        }
                        else
                        {
                            decimalPlace = num;
                            string newDisplayText = "0.";

                            for (int i = 0; i < num; i++)
                            {
                                newDisplayText += "0";
                            }

                            DecimalTextBox.Text = newDisplayText;
                            // Call the TextChanged method directly
                            numKeyPresses = 0;
                            check = 0;
                            DecimalTextBox_TextChanged(DecimalTextBox, new TextChangedEventArgs(TextBox.TextChangedEvent, UndoAction.None));

                        }
                    }
                }
                else
                {
                    feedback.Text = "Please enter integers only";
                    invalidCharEntry = true;
                }
            }
            
        }

            private void Slider_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            Slider slider = sender as Slider;
            if (slider != null)
            {
                double sliderValue = slider.Value;
                SliderOutput.Text = $"{sliderValue}";
            }
        }

        private void PopulateDropdownValues()
        {
            for (int i = 1; i <= 100; i++)
            {
                numberDropdown.Items.Add(i);
            }

            numberDropdown.SelectedIndex = 0;
        }

        private void ConversionInputChanged(object sender, TextChangedEventArgs e)
        {
            TextBox textBox = sender as TextBox;

            //check for unwanted characters

            bool containsNonNumbers = ContainsNonNumbers(textBox.Text, true);

            if (!containsNonNumbers)
            {
                feedback.Text = " ";
                invalidCharEntry = false;
            }
            else
            {
                feedback.Text = "Please enter integers only";
                invalidCharEntry = true;
            }

            //check if more than one decimal is entered
            int decimalCount = 0;

            foreach (char c in textBox.Text)
            {
                if (c.ToString().Equals("."))
                {
                    decimalCount++;
                }
            }

            if (decimalCount > 1)
            {
                feedback.Text = "Invalid entry";
                invalidWithMultipleDecimals = true;
            } else
            {
                invalidWithMultipleDecimals = false;
            }

        }

        private void CalculateConversion(string num)
        {
            if (num.Length >= 1) {

                feedback.Text = "";

                decimal dec = decimal.Parse(num);

                string fromUnit = FromUnit.SelectionBoxItem.ToString();
                string toUnit = ToUnit.SelectionBoxItem.ToString();

                //check for matching units 
                if (fromUnit.Equals(toUnit))
                {
                    feedback.Text = "Cannot convert a number to the same unit";
                    ConversionAns.Text = "";
                    return;
                }

                //Meters conversions
                if (fromUnit.Equals("Meters"))
                {
                    if (toUnit.Equals("Kilometers"))
                    {
                        conversionAnswer = dec / 1000;
                    }
                    else if (toUnit.Equals("Centimeters"))
                    {
                        conversionAnswer = dec * 100;
                    }
                    else if (toUnit.Equals("Millimeters"))
                    {
                        conversionAnswer = dec * 1000;
                    }
                    else if (toUnit.Equals("Yards"))
                    {
                        conversionAnswer = dec * 1.09361m;
                    }
                    else if (toUnit.Equals("Feet"))
                    {
                        conversionAnswer = dec * 3.28084m;
                    }
                    else if (toUnit.Equals("Inches"))
                    {
                        conversionAnswer = dec * 39.3701m;
                    }
                }

                //Kilometers conversions
                if (fromUnit.Equals("Kilometers"))
                {
                    if (toUnit.Equals("Meters"))
                    {
                        conversionAnswer = dec * 1000;
                    }
                    else if (toUnit.Equals("Centimeters"))
                    {
                        conversionAnswer = dec * 100000;
                    }
                    else if (toUnit.Equals("Millimeters"))
                    {
                        conversionAnswer = dec * 1000000;
                    }
                    else if (toUnit.Equals("Yards"))
                    {
                        conversionAnswer = dec * 1093.61m;
                    }
                    else if (toUnit.Equals("Feet"))
                    {
                        conversionAnswer = dec * 3280.84m;
                    }
                    else if (toUnit.Equals("Inches"))
                    {
                        conversionAnswer = dec * 39370.1m;
                    }
                }

                //Centimeters conversions
                if (fromUnit.Equals("Centimeters"))
                {
                    if (toUnit.Equals("Meters"))
                    {
                        conversionAnswer = dec / 100;
                    }
                    else if (toUnit.Equals("Kilometers"))
                    {
                        conversionAnswer = dec / 100000;
                    }
                    else if (toUnit.Equals("Millimeters"))
                    {
                        conversionAnswer = dec * 10;
                    }
                    else if (toUnit.Equals("Yards"))
                    {
                        conversionAnswer = dec / 91.44m;
                    }
                    else if (toUnit.Equals("Feet"))
                    {
                        conversionAnswer = dec / 30.48m;
                    }
                    else if (toUnit.Equals("Inches"))
                    {
                        conversionAnswer = dec / 2.54m;
                    }
                }

                //Millimeters conversions
                if (fromUnit.Equals("Millimeters"))
                {
                    if (toUnit.Equals("Meters"))
                    {
                        conversionAnswer = dec / 1000;
                    }
                    else if (toUnit.Equals("Kilometers"))
                    {
                        conversionAnswer = dec / 1000000;
                    }
                    else if (toUnit.Equals("Centimeters"))
                    {
                        conversionAnswer = dec / 10;
                    }
                    else if (toUnit.Equals("Yards"))
                    {
                        conversionAnswer = dec / 914.4m;
                    }
                    else if (toUnit.Equals("Feet"))
                    {
                        conversionAnswer = dec / 304.8m;
                    }
                    else if (toUnit.Equals("Inches"))
                    {
                        conversionAnswer = dec / 25.4m;
                    }
                }

                //Yards conversions
                if (fromUnit.Equals("Yards"))
                {
                    if (toUnit.Equals("Meters"))
                    {
                        conversionAnswer = dec / 1.09361m;
                    }
                    else if (toUnit.Equals("Kilometers"))
                    {
                        conversionAnswer = dec / 1093.61m;
                    }
                    else if (toUnit.Equals("Centimeters"))
                    {
                        conversionAnswer = dec * 91.44m;
                    }
                    else if (toUnit.Equals("Millimeters"))
                    {
                        conversionAnswer = dec * 914.4m;
                    }
                    else if (toUnit.Equals("Feet"))
                    {
                        conversionAnswer = dec * 3;
                    }
                    else if (toUnit.Equals("Inches"))
                    {
                        conversionAnswer = dec * 36;
                    }
                }

                //Feet conversions
                if (fromUnit.Equals("Feet"))
                {
                    if (toUnit.Equals("Meters"))
                    {
                        conversionAnswer = dec / 3.28084m;
                    }
                    else if (toUnit.Equals("Kilometers"))
                    {
                        conversionAnswer = dec / 3280.84m;
                    }
                    else if (toUnit.Equals("Centimeters"))
                    {
                        conversionAnswer = dec * 30.48m;
                    }
                    else if (toUnit.Equals("Millimeters"))
                    {
                        conversionAnswer = dec * 304.8m;
                    }
                    else if (toUnit.Equals("Yards"))
                    {
                        conversionAnswer = dec / 3;
                    }
                    else if (toUnit.Equals("Inches"))
                    {
                        conversionAnswer = dec * 12;
                    }
                }

                //Inches conversions
                if (fromUnit.Equals("Inches"))
                {
                    if (toUnit.Equals("Meters"))
                    {
                        conversionAnswer = dec / 39.3701m;
                    }
                    else if (toUnit.Equals("Kilometers"))
                    {
                        conversionAnswer = dec / 39370.1m;
                    }
                    else if (toUnit.Equals("Centimeters"))
                    {
                        conversionAnswer = dec * 2.54m;
                    }
                    else if (toUnit.Equals("Millimeters"))
                    {
                        conversionAnswer = dec * 25.4m;
                    }
                    else if (toUnit.Equals("Yards"))
                    {
                        conversionAnswer = dec / 36;
                    }
                    else if (toUnit.Equals("Feet"))
                    {
                        conversionAnswer = dec / 12;
                    }
                }

                conversionAnswer = Math.Round(conversionAnswer, 5);
                ConversionAns.Text = conversionAnswer.ToString();

            }
            
        }

        private void CalculateClicked(object sender, RoutedEventArgs e)
        {
            if (invalidCharEntry == false && invalidWithMultipleDecimals == false)
            {
                CalculateConversion(NumToBeConverted.Text);

            } else
            {
                feedback.Text = "Cannot calculate conversion for invalid entry";
            }
            
        }

        private void DecimalTextBox_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            var textBox = sender as TextBox;


            if (textBox != null && e.Text == ".")
            {
                //prevent period from being entered into textbox
                if (textBox.Text.Contains("."))
                {
                    e.Handled = true;
                }
            }
        }

        private void DecimalTextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            var textBox = sender as TextBox;

            var textBoxTest = DecimalTextBox;

            if (feedback != null)
            {
                if (textBox != null)
                {
                    bool containsNonNumbers = ContainsNonNumbers(textBox.Text, true);

                    if (!containsNonNumbers)
                    { 
                        feedback.Text = "";
                    }
                    else
                    {
                        feedback.Text = "Please enter integers only";
                        return;
                    }

                    //handle input display and decimal placement 

                    decBoxList.Clear();

                    foreach (char c in textBox.Text)
                    {
                        if (c != '.')
                        {
                            decBoxList.Add(c);
                        }
                    }

                    if (numKeyPresses != check)
                    {
                        if (numKeyPresses < decimalPlace + 2)
                        {
                            decBoxList.RemoveAt(0);
                        }
                    }

                    //display contents of decBoxList 

                    string displayString = "";

                    for (int i = 0; i < decBoxList.Count; i++)
                    {
                        displayString += decBoxList[i];
                    }

                    int insertDec = decBoxList.Count - decimalPlace;

                    displayString = displayString.Insert(insertDec, ".");

                    check = numKeyPresses;

                    DecimalTextBox.Text = displayString;
                    
                    textBox.Focus();
                    textBox.CaretIndex = textBox.Text.Length;
                }

            }

        }

        private void DecimalTextBox_PreviewMouseDown(object sender, MouseButtonEventArgs e)
        {
            //make cursor stay at end of number
            var textBox = sender as TextBox;

            if (textBox != null)
            {
                e.Handled = true;
                textBox.Focus();
                textBox.CaretIndex = textBox.Text.Length;
            }

        }

        private void DecimalTextBox_KeyDown(object sender, KeyEventArgs e)
        {
            var textBox = sender as TextBox;
            numKeyPresses++;

            if (textBox != null)
            {
                if (e.Key == Key.Left || e.Key == Key.Right || e.Key == Key.Home || e.Key == Key.End)
                {
                    //block arrow keys and other caret-moving keys
                    e.Handled = true;

                    //keep cursor at end of textbox
                    textBox.Focus();
                    textBox.CaretIndex = textBox.Text.Length;
                }

                if (e.Key == Key.Back)
                {
                    numKeyPresses--;
                    decBoxList.RemoveAt(decBoxList.Count - 1);

                    if (decBoxList.Count == 2)
                    {
                        decBoxList.Insert(0, '0');
                    }

                    string decimalDisplay = "";

                    foreach (char c in decBoxList)
                    {
                        decimalDisplay += c;
                    }

                    int insertDec = decBoxList.Count - decimalPlace;

                    decimalDisplay = decimalDisplay.Insert(insertDec, ".");


                    Dispatcher.BeginInvoke(new Action(() =>
                    {
                        // This code runs after the TextBox has updated its text
                        DecimalTextBox.Text = decimalDisplay;
                        // Use the updated text here
                    }));
                    
                }
            }
        }

        //ui code handling below
        private void UIButtonClicked(object sender, RoutedEventArgs e)
        {
            if (!viewIsEnlarged)
            {
                IncreaseUIElementSizes(this, 4);
                IncreaseRowMargins(this, 1);
                viewIsEnlarged = true;
            } else
            {
                RestartApplication();
            }
            
        }

        private void IncreaseUIElementSizes(DependencyObject parent, double sizeIncreaseFactor)
        {
            //targets window size
            if (this.Width > 0)
            {
                this.Width += sizeIncreaseFactor * .4;
            }

            if (this.Height > 0)
            {
                this.Height = 675;
            }

            for (int i = 0; i < VisualTreeHelper.GetChildrenCount(parent); i++)
            {
                var child = VisualTreeHelper.GetChild(parent, i);

                if (child is TextBlock textBlock)
                {
                    if (textBlock.Name == "feedback")
                    {
                        //params in order left, top, right, bottom
                        textBlock.Margin = new Thickness(15, 10, 10, 10);
                    }

                    textBlock.FontSize += sizeIncreaseFactor;
                }

                if (child is TextBox textBox)
                {
                    //params in order left, top, right, bottom
                    textBox.Margin = new Thickness(10, 5, 10, 15);
                    textBox.FontSize += sizeIncreaseFactor * 2;
                    textBox.Width += sizeIncreaseFactor * 4;
                    textBox.Height += sizeIncreaseFactor * 4;
                }

                if (child is IntegerUpDown intUpDown)
                {
                    intUpDown.FontSize += sizeIncreaseFactor;
                    intUpDown.Width += sizeIncreaseFactor * 26;
                    intUpDown.Height += sizeIncreaseFactor * 4;
                    intUpDown.Margin = new Thickness(5, 10, 5, 5);
                    intUpDown.Padding = new Thickness(-7);
                }

                if (child is Button button)
                {
                    button.FontSize += sizeIncreaseFactor * .8;
                    button.Width += sizeIncreaseFactor * 10;
                    button.Height += sizeIncreaseFactor * 3.2;
                    button.Margin = new Thickness(10, 10, 10, 10);

                    if (button.Name == "UIButton")
                    {
                        button.Content = "Revert";
                        button.Margin = new Thickness(10, 0, 10, 10);
                    }
                }

                if (child is ComboBox comboBox)
                {
                    comboBox.FontSize += sizeIncreaseFactor * .8;
                    comboBox.Width += sizeIncreaseFactor * 26;
                    comboBox.Height += sizeIncreaseFactor * 3.2;
                    comboBox.Margin = new Thickness(5, 10, 5, 10);
                }

                if (child is Slider slider)
                {
                    slider.Width += sizeIncreaseFactor * 50;
                    slider.HorizontalAlignment = HorizontalAlignment.Center;
                }

                if (child is FrameworkElement element)
                {
                    IncreaseUIElementSizes(element, sizeIncreaseFactor);
                }
            }
        }

        private void IncreaseRowMargins(DependencyObject parent, double sizeIncreaseFactor)
        {
            for (int i = 0; i < VisualTreeHelper.GetChildrenCount(parent); i++)
            {
                var child = VisualTreeHelper.GetChild(parent, i);

                if (child is Grid grid)
                {
                    for (int rowIndex = 0; rowIndex < grid.RowDefinitions.Count; rowIndex++)
                    {
                        var rowDef = grid.RowDefinitions[rowIndex];

                        if (rowDef.Height.IsStar)
                        {
                            rowDef.Height = new GridLength(rowDef.Height.Value + sizeIncreaseFactor * 0.1, GridUnitType.Star);
                        }
                        else if (rowDef.Height.IsAbsolute)
                        {
                            rowDef.Height = new GridLength(rowDef.Height.Value + sizeIncreaseFactor, GridUnitType.Pixel);
                        }
                        else if (rowDef.Height.IsAuto)
                        {
                            rowDef.Height = new GridLength(rowDef.Height.Value + sizeIncreaseFactor, GridUnitType.Pixel);
                        }
                    }
                }
            }
        }

        private void RestartApplication()
        {
            //get current applications process start information
            var currentProcess = System.Diagnostics.Process.GetCurrentProcess();
            var startInfo = new System.Diagnostics.ProcessStartInfo
            {
                FileName = currentProcess.MainModule?.FileName,
                Arguments = string.Join(" ", Environment.GetCommandLineArgs().Skip(1)),
                UseShellExecute = true
            };

            //start new instance of application
            System.Diagnostics.Process.Start(startInfo);

            //shut down current application
            Application.Current.Shutdown();
        }
    }
}