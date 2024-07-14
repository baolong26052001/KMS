using System.Text.RegularExpressions;

public class VietnameseConverter
{
    public static string ConvertToSimpleForm(string input)
    {
        // Create a regex pattern to match Vietnamese diacritics
        string pattern = "[áàảãạăắằẳẵặâấầẩẫậéèẻẽẹêếềểễệíìỉĩịóòỏõọôốồổỗộơớờởỡợúùủũụưứừửữựýỳỷỹỵđÁÀẢÃẠĂẮẰẲẴẶÂẤẦẨẪẬÉÈẺẼẸÊẾỀỂỄỆÍÌỈĨỊÓÒỎÕỌÔỐỒỔỖỘƠỚỜỞỠỢÚÙỦŨỤƯỨỪỬỮỰÝỲỶỸỴĐ]";

        // Use Regex.Replace to remove diacritics from the input
        string result = Regex.Replace(input, pattern, m => VietnameseConverter.RemoveDiacritics(m.Value));

        return result;
    }

    // Method to remove diacritics from a single character
    private static string RemoveDiacritics(string input)
    {
        // Diacritics and their corresponding characters
        string diacritics = "áàảãạăắằẳẵặâấầẩẫậéèẻẽẹêếềểễệíìỉĩịóòỏõọôốồổỗộơớờởỡợúùủũụưứừửữựýỳỷỹỵđÁÀẢÃẠĂẮẰẲẴẶÂẤẦẨẪẬÉÈẺẼẸÊẾỀỂỄỆÍÌỈĨỊÓÒỎÕỌÔỐỒỔỖỘƠỚỜỞỠỢÚÙỦŨỤƯỨỪỬỮỰÝỲỶỸỴĐ";
        string characters = "aaaaaaaaaaaaaaaaaeeeeeeeeeeeiiiiiooooooooooooooooouuuuuuuuuuuyyyyydAAAAAAAAAAAAAAAAAEEEEEEEEEEEIIIIIOOOOOOOOOOOOOOOOOUUUUUUUUUUUYYYYYD";

        // Find index of input character in diacritics
        int index = diacritics.IndexOf(input);

        // If found, return corresponding character from characters string, otherwise return input
        return index >= 0 ? characters[index].ToString() : input;
    }
}
