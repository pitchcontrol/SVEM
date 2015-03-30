namespace SMEVService
{
    /// <summary>
    /// Статус сообщения
    /// </summary>
    public enum StatusType
    {
        /// <summary>
        /// Запрос
        /// </summary>
        REQUEST,
        /// <summary>
        /// Результат
        /// </summary>
        RESULT,
        /// <summary>
        /// Мотивированный отказ
        /// </summary>
        REJECT,
        /// <summary>
        /// Ошибка при ФЛК
        /// </summary>
        INVALID,
        /// <summary>
        /// Сообщение-квиток о приеме
        /// </summary>
        ACCEPT,
        /// <summary>
        /// Запрос данных/результатов
        /// </summary>
        PING,
        /// <summary>
        /// В обработке
        /// </summary>
        PROCESS,
        /// <summary>
        /// FAILURE
        /// </summary>
        NOTIFY,
        /// <summary>
        /// Технический сбой
        /// </summary>
        FAILURE,
        /// <summary>
        /// Отзыв заявления
        /// </summary>
        CANCEL,
        /// <summary>
        /// Возврат состояния
        /// </summary>
        STATE,
        /// <summary>
        /// Передача пакетного сообщения
        /// </summary>
        PACKET
    }
}