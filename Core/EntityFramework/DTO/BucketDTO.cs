 public class BucketDTO
    {
        public ISet<int> ProjectIds {get; set;}

        [Required]
        public string Key {get; set;}

        public BucketDTO(ISet<int> ProjectIds,string Key) {
            this.ProjectIds = ProjectIds;
            this.Key = Key;
        }
    }
public record BucketCreateDTO() {
    public ISet<int> ProjectIds {get; init;}
    
    [Required]
    public string Key {get; init;}

}