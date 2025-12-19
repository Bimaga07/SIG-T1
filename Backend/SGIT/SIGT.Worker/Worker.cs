namespace SIGT.Worker
{
    public class Worker : BackgroundService
    {
        private readonly ILogger<Worker> _logger;

        public Worker(ILogger<Worker> logger)
        {
            _logger = logger;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            _logger.LogInformation("Servicio de Segundo Plano iniciado.");

            while (!stoppingToken.IsCancellationRequested)
            {
                // REQUISITO: Bloque Try-Catch para que el servicio no muera ante un error
                try
                {
                    _logger.LogInformation("Worker procesando tareas: {time}", DateTimeOffset.Now);

                    // Simulación de trabajo pesado (ej. generar un PDF o reporte)
                    await Task.Delay(2000, stoppingToken);

                    _logger.LogInformation("Procesamiento completado con éxito.");

                    // REQUISITO: Polling de 30 segundos (30,000 milisegundos)
                    await Task.Delay(30000, stoppingToken);
                }
                catch (Exception ex)
                {
                    // Si algo falla, lo logueamos pero el 'while' permite que siga intentándolo
                    _logger.LogError(ex, "Error en el ciclo del Worker. Reintentando en la siguiente iteración.");
                }
            }
        }
    }
}