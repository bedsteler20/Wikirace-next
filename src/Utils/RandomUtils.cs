namespace Wikirace.Utils;

public static class RandomUtils {
    public static string CreateCode(int length) => CreateCode(length, "ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789");

    public static string CreateCode(int length, string chars) {
        var random = new Random();
        var code = new char[length];
        for (var i = 0; i < length; i++) {
            code[i] = chars[random.Next(chars.Length)];
        }

        return new string(code);
    }
}