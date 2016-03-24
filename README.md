# Gramma.BetaImport
This is a simple .NET library to convert ASCII strings containing "beta code"-encoded ancient Greek texts to Unicode. Both composing and precombined diacritics are supported.

These two converters are derived from the abstract `BetaConverter`, named `ComposingDiacriticsBetaConverter` and `PrecombinedDiacriticsBetaConverter` respectively. The inherited method `Convert` can be used directly, but they can also be combined with `BetaReader`, a descendant of the .NET standard `TextReader` for text streaming scenarios. As a simple example, the following WPF code fragment uses a file dialog to open a beta code file and place its contents in a text box:

```cs
var dialog = new Microsoft.Win32.OpenFileDialog();

dialog.DefaultExt = ".txt";
dialog.Filter = "Text documents|*.txt";

if (dialog.ShowDialog(this) == true)
{
	using (Stream inputStream = dialog.OpenFile())
	{
		using (var betaReader = new BetaReader(inputStream, new PrecombinedDiacriticsBetaConverter()))
		//using (var betaReader = new BetaReader(inputStream, new ComposingDiacriticsBetaConverter()))
		{
			fileTextBox.Text = dialog.FileName;
			contentTextBox.ScrollToHome();
			contentTextBox.Text = betaReader.ReadToEnd();
		}
	}
}
```

The above code fragment is part of a simple demo WPF application used to view contents of beta code files. It can be found [here](https://github.com/grammophone/Gramma.BetaImport.Viewer).

The library has no dependencies.
