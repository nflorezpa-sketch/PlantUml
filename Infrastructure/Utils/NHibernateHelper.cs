using NHibernate;
using NHibernate.Cfg;

namespace Infrastructure.Utils;

public class NHibernateHelper
{
    private static ISessionFactory? _sessionFactory;
    private static readonly object _lock = new object();

    public static ISessionFactory SessionFactory
    {
        get
        {
            if (_sessionFactory == null)
            {
                lock (_lock)
                {
                    if (_sessionFactory == null)
                    {
                        _sessionFactory = BuildSessionFactory();
                    }
                }
            }
            return _sessionFactory;
        }
    }

    private static ISessionFactory BuildSessionFactory()
    {
        var configuration = new Configuration();
        
        var baseDir = AppContext.BaseDirectory;
        var configPath = Path.Combine(baseDir, "NHibernate.cfg.xml");
        
        if (!File.Exists(configPath))
        {
            configPath = Path.Combine(baseDir, "NHibernate", "NHibernate.cfg.xml");
        }
        
        if (File.Exists(configPath))
        {
            configuration.Configure(configPath);
        }
        else
        {
            configuration.Configure();
        }

        return configuration.BuildSessionFactory();
    }

    public static ISession OpenSession()
    {
        return SessionFactory.OpenSession();
    }
}
