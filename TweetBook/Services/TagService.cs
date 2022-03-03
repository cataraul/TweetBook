using TweetBook.Domain;

namespace TweetBook.Services
{
    public class TagService : ITagsService <Tag,string>
    {
        private readonly DataContext _dataContext;

        public TagService(DataContext dataContext)
        {
            _dataContext = dataContext;
        }

        public async Task<IEnumerable<Tag>> GetAllAsync()
        {
            return await _dataContext.Tags.AsNoTracking().ToListAsync();
        }

        public async Task<bool> DeleteAsync(string tagName)
        {
            var tagToDelete = await _dataContext.Tags.FirstOrDefaultAsync(tag => tag.Name == tagName);

            if (tagToDelete == null)
            {
                return false;
            }

            _dataContext.Tags.Remove(tagToDelete);
            var numDeleted = await _dataContext.SaveChangesAsync();

            return numDeleted > 0;
        }
        public async Task<Tag> GetAsync(string name)
        {
            return await _dataContext.Tags.FirstOrDefaultAsync(tag => tag.Name == name);
        }
        public async Task<bool> CreateAsync(Tag tag)
        {
            _dataContext.Tags.Add(tag);
            var created = await _dataContext.SaveChangesAsync();

            return created > 0;
        }
        public async Task<bool> UpdateAsync(Post updatedPost)
        {

            await AddNewTagsAsync(updatedPost);
           _dataContext.Posts.Update(updatedPost);
            var numUpdated = await _dataContext.SaveChangesAsync();

            return numUpdated > 0;
        }
        private async Task AddNewTagsAsync(Post post)
        {
            foreach (var newTag in post.Tags)
            {
                var matchingTag = await _dataContext.Tags.SingleOrDefaultAsync(existingTag => existingTag.Name == newTag.TagName);

                if (matchingTag != null)
                {
                    continue;
                }

                _dataContext.Tags.Add(new Tag
                {
                    Name = newTag.TagName,
                    CreatorId = post.UserId,
                    CreatedOn = DateTime.UtcNow
                });
            }
        }

    }
}
