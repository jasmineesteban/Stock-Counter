namespace MauiApp1.Helpers
{
    public static class EntryFormatHelper
    {
        public static void ExpiryEntry_TextChanged(object sender, TextChangedEventArgs e)
        {
            var entry = (Entry)sender;
            var text = entry.Text;

            string cleanedText = new string(text.Where(char.IsDigit).ToArray());

            if (cleanedText.Length > 8)
            {
                cleanedText = cleanedText.Substring(0, 8);
            }

            if (cleanedText.Length > 4)
            {
                cleanedText = cleanedText.Insert(4, "-");
            }
            if (cleanedText.Length > 7)
            {
                cleanedText = cleanedText.Insert(7, "-");
            }

            entry.Text = cleanedText;
            entry.CursorPosition = cleanedText.Length;
        }
    }
}
