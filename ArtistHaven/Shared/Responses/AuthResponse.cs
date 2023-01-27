namespace ArtistHaven.Shared.Responses {
    public class AuthResponse : BasicResponse {
        public IEnumerable<string> Errors { get; set; } = new List<string>();
        
        public override string ToString() {
            string res = "";
            if (Message != null)
                res += Message + "\n";

            res += string.Join("\n", Errors);
            return res;
        }
    }
}
