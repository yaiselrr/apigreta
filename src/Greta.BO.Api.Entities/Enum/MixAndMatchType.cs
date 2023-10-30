namespace Greta.BO.Api.Entities.Enum
{
    public enum MixAndMatchType
    {
        /// <summary>
        ///     This is one of the most used discounts. When the first item is scanned,
        ///     we would charge the retail price or sale price for the item. When the second
        ///     UPC is scanned again and we see it has already been added to the receipt the
        ///     second one scanned is discounted 100% or is added to the order at 0.00.
        ///     Tax is added to the receipt to buy the item scanned.
        ///     In this case, when the first item is scanned, we would apply all taxes that are
        ///     assigned to the item in the product record. Based on the retail price or sale
        ///     price of the item. Since the second item is scanned at 0.00 the tax charged is 0.00.
        ///     When this type is selected you would show a QTY selection 0f a whole number.
        ///     We have to allow A Pricing Family. So if the first item is part of the family.
        ///     Then when the second item that is part of the Family is scanned it would be charged at 0.00 price.
        ///     Spanish
        ///     Este es uno de los descuentos más utilizados. Cuando se escanea el primer producto, cobraremos el
        ///     precio de venta al público. Cuando se vuelve a escanear el segundo UPC del mismo producto y vemos
        ///     que ya se ha añadido al recibo, el segundo producto escaneado se descuenta al 100% o se añade al pedido
        ///     por precio de  $0.00. El impuesto se agrega al recibo para comprar el producto escaneado.
        ///     En este caso, cuando se escanea el primer producto, aplicaríamos todos los impuestos que se asignan
        ///     al producto en el registro del producto. Según el precio minorista o el precio de venta del
        ///     producto. Dado que el segundo producto se escanea a 0,00, el impuesto cobrado es 0,00.
        ///     Cuando se selecciona este tipo, se mostrará una selección de CANTIDAD de un número entero.
        ///     Tenemos que permitir una familia de precios. Entonces, si el primer producto es parte de la familia.
        ///     Luego, cuando se escanea el segundo producto que forma parte de la familia, se le cobrará un precio de $0.00.
        /// </summary>
        BuyAndGetFree,

        /// <summary>
        ///     This is where we apply a discount to a product at the time of sale. If a product has a 50% discount, when
        ///     the product is scanned, we will calculate the amount of the discount on the receipt. An example product
        ///     is 1.00, the discount is 50%, so we would add .50 to the receipt and tax on the .50. If the discount
        ///     percentage is set in the purchase of multiples and a discount percentage is obtained, we would do
        ///     the following. If the product is 3 and you get a 50% discount, we would calculate it as follows.
        ///     The first product would be added to the receipt at 1.00, the second would be scanned and added
        ///     to 1.00 as we scan, the product tax would be added to the receipt. When the third item is scanned,
        ///     we will do the calculations to arrive at the discount percentage, so if you look at this example, we
        ///     will add the 3 items up to 3.00, then we will take the 50% discount, which is 1.50 and then we
        ///     will set the second. the product is 1.00, so we would set it to .50 and charge the tax on the
        ///     receipt of .50. We will then charge 0.00 for the third product scanned. Since the product is set to 0.00,
        ///     there would be no tax.
        ///     (Spanish)
        ///     Aquí es donde aplicamos un descuento a un producto en el momento de la venta. Si un producto tiene un
        ///     descuento del 50%, cuando se escanee el producto, calcularemos el monto del descuento que figura en
        ///     el recibo. Un producto de ejemplo es 1.00, el descuento es del 50%, por lo que agregaríamos .50 al
        ///     recibo y cobraríamos impuestos sobre el .50. Si el porcentaje de descuento se establece en la compra
        ///     de múltiplos y se obtiene un porcentaje de descuento, haríamos lo siguiente. Si el producto es 3 y
        ///     obtiene un 50% de descuento, lo calcularíamos de la siguiente manera. El primer producto se agregaría
        ///     al recibo en 1.00, el segundo se escanearía y se agregaría a 1.00 a medida que escaneamos, el impuesto
        ///     del producto se agregaría al recibo. Cuando se escanea el tercer elemento, haremos los cálculos para llegar
        ///     al porcentaje de descuento, por lo que si miras este ejemplo, agregaremos los 3 elementos hasta 3.00,
        ///     luego tomaremos el 50% de descuento, que es 1.50 y luego configuraremos el segundo. el producto es 1,00,
        ///     por lo que lo estableceríamos en .50 y cargaríamos el impuesto en el recibo de .50. Entonces cobraremos
        ///     0,00 por el tercer producto escaneado. Dado que el producto se establece en 0,00, no habría impuestos.
        /// </summary>
        DiscountPercentage,

        /// <summary>
        ///     If you have a product that sells for 3 for $ 2.00 and the retail product is $ 1.25, then each product
        ///     will be added to the receipt at $ 1.25 when the third Product is added to the receipt, we would sum the
        ///     3 retail products and subtract to get the sale amount. The product would be 1.25, the second scan would
        ///     be set to .75 and the third would be set to 0.00 if the Product is taxable, the tax would be added.
        ///     (Spanish)
        ///     Si tiene un producto que se vende a 3 por $2.00 y el producto al por menor es $1.25, entonces cada
        ///     producto se agregará al recibo a $1.25 cuando se agregue el tercer Producto al recibo, sumaríamos
        ///     los 3 productos al por menor y restaríamos para obtener el monto de la venta. El producto sería 1.25,
        ///     el segundo escaneado se establecería en .75 y el tercero se establecería en 0.00 si el Producto está
        ///     sujeto a impuestos, se agregaría el impuesto.
        /// </summary>
        FixedPriceDiscount,

        /// <summary>
        ///     Here is an example of what it would be. I buy a sandwich for 2.50 and then I would get multiple items
        ///     as they are scanned free. When the sandwich is scanned, we would add 2.50 to the receipt and charge
        ///     tax for that item. If I had a buy sandwich and get 2 items free. When the free item is scanned, it
        ///     would be added to the receipt in this case it would be set at 0.00. The free items are only added to
        ///     the receipt if they are scanned.
        ///     (Spanish)
        ///     Aquí hay un ejemplo de lo que sería. Compro un sándwich por 2,50 y luego obtengo varios productos,
        ///     ya que se escanean gratis. Cuando se escanea el sándwich, agregaríamos 2.50 al recibo y cobraremos
        ///     impuestos por ese producto. Si tuviera un sándwich comprar y obtener 2 productos gratis. Cuando se
        ///     escanea el producto gratuito, se agregaría al recibo, en este caso se establecería en 0.00.
        ///     Los productos gratuitos solo se agregan al recibo si se escanean.
        /// </summary>
        BuyOneGetFree,

        /// <summary>
        ///     This would-be setup like this. I have a group of items. Whole Turkey, Box of Stuffing, 1 pound of sausage.
        ///     If these items are scanned, then I would get 10.00 off the Subtotal of the receipt. If the subtotal of
        ///     the receipt is 50.00 and I purchased all items required then I would receive a 10.00 discount So my
        ///     order Total would now be 40.00 Since all the taxes have been added as each item is scanned there would
        ///     be no changes to tax.
        ///     (Spanish)
        ///     Esta posible configuración como esta. Tengo un grupo de productos. Pavo entero, caja de relleno, 1 libra
        ///     de salchicha. Si se escanean estos productos, obtendría 10,00 de descuento en el subtotal del recibo.
        ///     si el subtotal del recibo es 50,00 y compré todos los productos requeridos, entonces recibiría un
        ///     descuento de 10,00 Por lo tanto, el total de mi pedido ahora sería 40,00 Dado que todos los impuestos
        ///     se han agregado a medida que se escanea cada producto, no habría cambios en los impuestos.
        /// </summary>
        BuyItemAndCredit,

        /// <summary>
        ///     This would be for items that have the price embedded in the barcode. I scan the first item and it is 22.35
        ///     then I scan the second item 29.42 since the first item scanned is 22.35, I would set the price to 0.00
        ///     for it. It does not matter what order the items are scanned but the lesser value will always be discounted
        ///     and set to 0.00 If any taxes were charged on the discounted item they would be subtracted from the receipt.
        ///     (Spanish)
        ///     Esto sería para los productos que tienen el precio incrustado en el código de barras. Escaneo el primer
        ///     producto y es 22.35, luego escaneo el segundo producto 29.42 ya que el primer producto escaneado es 22.35,
        ///     fijaría el precio en 0.00 para él. No importa en qué orden se escaneen los productos, pero el valor menor
        ///     siempre se descontará y se establecerá en 0.00. Si se cobraron impuestos sobre el producto con descuento,
        ///     se restarían del recibo.
        /// </summary>
        BuyOneGetOneCheapFree
    }
}