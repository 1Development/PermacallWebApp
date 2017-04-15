using CodeKicker.BBCode;

namespace PermacallWebApp.Logic
{
    public class BBCode
    {
        private static BBCodeParser Parser;

        public static string ParseBBCode(string input)
        {
            if (Parser == null) InitParser();
            if (input == null) return "";
            input = input.Replace("\r\n", "[br]");
            return Parser?.ToHtml(input);
        }




        private static void InitParser()
        {
            Parser = new BBCodeParser(new[]
                {
                    new BBTag("h1", "<h1>", "</h1>"),
                    new BBTag("h2", "<h2>", "</h2>"),
                    new BBTag("h3", "<h3>", "</h3>"),
                    new BBTag("h4", "<h4>", "</h4>"),
                    new BBTag("br", "<br/>", "", true, false),
                    new BBTag("b", "<b>", "</b>"),
                    new BBTag("i", "<span style=\"font-style:italic;\">", "</span>"),
                    new BBTag("u", "<span style=\"text-decoration:underline;\">", "</span>"),
                    new BBTag("code", "<pre class=\"prettyprint\">", "</pre>"),
                    new BBTag("img", "<img class=\"ContentImg\" src=\"${content}\" />", "", false, true),
                    new BBTag("fa", "<i class=\"fa ${content}\" aria-hidden=\"true\"></i>", "", false, true),
                    new BBTag("quote", "<blockquote>", "</blockquote>"),
                    new BBTag("list", "<ul>", "</ul>"),
                    new BBTag("*", "<li>", "</li>", true, false),
                    new BBTag("size", "<span style=\"font-size:${size}%\">", "</span>", new BBAttribute("size", ""), new BBAttribute("size", "size")),
                    new BBTag("color", "<span style=\"color:${clr}\">", "</span>", new BBAttribute("clr", ""), new BBAttribute("clr", "clr")),
                    new BBTag("url", "<a href=\"${href}\">", "</a>", new BBAttribute("href", ""), new BBAttribute("href", "href")),
                });
        }
    }
}