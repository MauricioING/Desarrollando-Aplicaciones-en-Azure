// See https://aka.ms/new-console-template for more information

using Azure.Storage.Queues;
using Azure.Storage.Queues.Models;

var connectionStringQueue = "BlobEndpoint=https://storageaprendizajeprod.blob.core.windows.net/;QueueEndpoint=https://storageaprendizajeprod.queue.core.windows.net/;FileEndpoint=https://storageaprendizajeprod.file.core.windows.net/;TableEndpoint=https://storageaprendizajeprod.table.core.windows.net/;SharedAccessSignature=sv=2024-11-04&ss=q&srt=o&sp=rdlp&se=2026-03-12T21:09:29Z&st=2025-12-12T12:54:29Z&spr=https&sig=mN%2FEdmNPP8BpOf2NUwZEtyMaxyfFbOKmADCkBxfwhX0%3D";

var nombreQueue = "ejemplo";

QueueClient queueClient = new QueueClient(connectionStringQueue, nombreQueue);

Console.WriteLine("Procesando mensajes....");

while (true)
{
    QueueMessage[] mensajes = await queueClient.ReceiveMessagesAsync();

	foreach (var mensaje in mensajes)
	{
        Console.WriteLine($"Mensaje: {mensaje.MessageText}");
        await queueClient.DeleteMessageAsync(mensaje.MessageId, mensaje.PopReceipt);
    }

    await Task.Delay(TimeSpan.FromSeconds(5));
}