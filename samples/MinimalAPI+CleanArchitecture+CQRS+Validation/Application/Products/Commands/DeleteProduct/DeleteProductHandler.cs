using Application.Common;
using Application.Common.Exceptions;


namespace Application.Products.Commands.DeleteProduct;

internal class DeleteProductHandler(
    IProductRepository repository
) : ICommandHandler<DeleteProductCommand, bool>
{
    public async ValueTask<CommandResult<bool>> Handle(DeleteProductCommand request, CancellationToken cancellationToken)
    {
        if (!await repository.ProductExists(request.Id, cancellationToken))
        {
            throw new NotFoundException($"The product with ID='{request.Id}' not found.");
        }

        return await repository.DeleteProduct(request.Id, cancellationToken);
    }
}
