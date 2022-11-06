import cl from "./ReportCard.module.scss";
import { FontAwesomeIcon } from "@fortawesome/react-fontawesome";
import {
  faFilePdf,
  faFileExcel,
  faClock,
} from "@fortawesome/free-solid-svg-icons";
import { doDownloadPDF } from "../../../api/Auth";

export const ReportCard = ({
  name,
  pending,
  id,
  createDate,
  pdfUrl,
  excelUrl,
}) => {
  const downloadPdf = () => {
    doDownloadPDF(pdfUrl);
  };
  const downloadExcel = () => {
    doDownloadPDF(pdfUrl);
  };

  return (
    <div className={[cl.report_card, pending ? cl.pending : ""].join(" ")}>
      <div className={cl.header}>
        <h3>{name}</h3>
        <div className={cl.date}>
          <FontAwesomeIcon icon={faClock} />
          <p>Запрошено: {createDate}</p>
        </div>
      </div>
      <div className={cl.body}>
        <button className={cl.excel} onClick={downloadExcel}>
          <FontAwesomeIcon icon={faFileExcel} />
          <p>Скачать Excel</p>
        </button>
        <button className={cl.pdf} onClick={downloadPdf}>
          <FontAwesomeIcon icon={faFilePdf} />
          <p>Скачать PDF</p>
        </button>
      </div>
    </div>
  );
};
