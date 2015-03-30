namespace SMEVService
{
    /// <summary>
    /// ������ ���������
    /// </summary>
    public enum StatusType
    {
        /// <summary>
        /// ������
        /// </summary>
        REQUEST,
        /// <summary>
        /// ���������
        /// </summary>
        RESULT,
        /// <summary>
        /// �������������� �����
        /// </summary>
        REJECT,
        /// <summary>
        /// ������ ��� ���
        /// </summary>
        INVALID,
        /// <summary>
        /// ���������-������ � ������
        /// </summary>
        ACCEPT,
        /// <summary>
        /// ������ ������/�����������
        /// </summary>
        PING,
        /// <summary>
        /// � ���������
        /// </summary>
        PROCESS,
        /// <summary>
        /// FAILURE
        /// </summary>
        NOTIFY,
        /// <summary>
        /// ����������� ����
        /// </summary>
        FAILURE,
        /// <summary>
        /// ����� ���������
        /// </summary>
        CANCEL,
        /// <summary>
        /// ������� ���������
        /// </summary>
        STATE,
        /// <summary>
        /// �������� ��������� ���������
        /// </summary>
        PACKET
    }
}