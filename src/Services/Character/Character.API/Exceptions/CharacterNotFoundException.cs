namespace Character.API.Exceptions
{
    public class CharacterNotFoundException : NotFoundException
    {
        public CharacterNotFoundException(Guid Id) : base(nameof(Entities.Character), Id)
        {          
        }

        public CharacterNotFoundException(string message) : base(message)
        {        
        }
    }
}
