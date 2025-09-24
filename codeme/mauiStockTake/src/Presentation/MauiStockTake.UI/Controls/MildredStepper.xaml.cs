namespace MauiStockTake.UI.Controls;

public partial class MildredStepper : ContentView
{
    #region ValueProperty
    public static readonly BindableProperty ValueProperty = BindableProperty.Create(
        nameof(Value),
        typeof(int),
        typeof(MildredStepper),
        0,
        BindingMode.TwoWay,
        propertyChanged: OnValueChanged);

    public int Value
    {
        get => (int)GetValue(ValueProperty);
        set
        {
            SetValue(ValueProperty, value);
            if (value < 1)
            {
                MinusButton.IsEnabled = false;
            }
            else
            {
                MinusButton.IsEnabled = true;
            }
        }
    }

    private static void OnValueChanged(BindableObject bindable, object oldValue, object newValue)
    {
        if (bindable is MildredStepper mildredStepper)
        {
            mildredStepper.Value = (int)newValue;
            mildredStepper.ValueEntry.Text = mildredStepper.Value.ToString();
        }
    }
    #endregion

    #region IsEnabledProperty
    public static new readonly BindableProperty IsEnabledProperty = BindableProperty.Create(
        nameof(IsEnabled),
        typeof(bool),
        typeof(MildredStepper),
        true,
        BindingMode.TwoWay,
        propertyChanged: OnIsEnabledChanged);

    private static void OnIsEnabledChanged(BindableObject bindable, object oldValue, object newValue)
    {
        if (bindable is MildredStepper mildredStepper)
        {
            mildredStepper.IsEnabled = (bool)newValue;
            mildredStepper.ValueEntry.IsEnabled = mildredStepper.IsEnabled;
            mildredStepper.PlusButton.IsEnabled = mildredStepper.IsEnabled;
            mildredStepper.MinusButton.IsEnabled = mildredStepper.IsEnabled;
        }
    }
    #endregion

    public MildredStepper()
    {
        InitializeComponent();
        ValueEntry.Text = "0";
    }

    private void MinusButton_Clicked(object sender, EventArgs e)
    {
        if (Value > 0)
        {
            Value--;
            ValueEntry.Text = Value.ToString();
        }
    }

    private void PlusButton_Clicked(object sender, EventArgs e)
    {
        Value++;
        ValueEntry.Text = Value.ToString();
    }

    private void ValueEntry_TextChanged(object sender, TextChangedEventArgs e)
    {
        if (e.NewTextValue.StartsWith("-"))
        {
            ValueEntry.Text = Value.ToString();
            return;
        }
        
        if (int.TryParse(e.NewTextValue, out var value))
        {
            Value = value;
        }
    }
}