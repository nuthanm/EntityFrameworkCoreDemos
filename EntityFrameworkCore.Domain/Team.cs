namespace EntityFrameworkCore.Domain
{
    public class Team
    {
        public int Id { get; set; }

        public string Name { get; set; } = string.Empty;

        // <TableName><ColumnName> => ForiegnKey
        public int LeagueId { get; set; }

        // Navigation Property
        public virtual League League { get; set; }

        // LeagueId and League consider EFCore is nothing for foriegnKey relationship

    }
}