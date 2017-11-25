namespace DatabaseAccess.Repositories
{
    public abstract class Repository
    {
        protected FoxyFaceDB FoxyFaceDb;
        
        protected Repository(FoxyFaceDB foxyFaceDb)
        {
            FoxyFaceDb = foxyFaceDb;
        }
    }
}