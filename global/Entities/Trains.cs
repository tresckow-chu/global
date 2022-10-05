namespace global.Entities
{
    public class Book
    {
        public int BookId { get; set; }
        public string? BookName { get; set; }
        public string? BookJsonFileName
        {
            get
            {

                string bk = BookName.Replace(" ", "");

                return $"{bk}.json";
            }
        }

    }

    public class Chapter
    {
        public string? ChapterName { get; set; }
        public int ChapterId { get; set; }
        public int BookId { get; set; }
    }

    public class Project
    {
        public int Id { get; set; }
        public string? Name { get; set; }
    }

    public class Verse
    {
        public int Id { get; set; }
        public string? Name { get; set; }
    }

    public class VerseMike
    {
        public string? Book { get; set; }
        public IEnumerable<VerseDetail>? Chapters { get; set; }
    }

    public class VerseDetail
    {
        public string? Chapter { get; set; }
        public IEnumerable<VerseLong>? Verses { get; set; }
    }

    public class VerseLong
    {
        public string? Verse { get; set; }
        public string? Text { get; set; }

    }
}
