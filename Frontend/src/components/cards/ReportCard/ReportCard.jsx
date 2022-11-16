import cl from "./ReportCard.module.scss";
import { FontAwesomeIcon } from "@fortawesome/react-fontawesome";
import {
  faFilePdf,
  faFileExcel,
  faClock,
} from "@fortawesome/free-solid-svg-icons";
import { doDownloadExcel, doDownloadPDF } from "../../../api/Auth";
import { useState } from "react";

export const ReportCard = ({
  name,
  pending,
  id,
  createDate,
  pdfUrl,
  excelUrl,
}) => {
  const [pdfDisabled, setPdfDisabled] = useState(false);
  const [excelDisabled, setExcelDisabled] = useState(false);

  const downloadPdf = () => {
    if (pending) {
      return;
    }
    setPdfDisabled(true);
    doDownloadPDF(pdfUrl, name).then(() => {
      setPdfDisabled(false);
    });
  };

  const downloadExcel = () => {
    if (pending) {
      return;
    }
    setExcelDisabled(true);
    doDownloadExcel(excelUrl, name).then(() => {
      setExcelDisabled(false);
    });
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
        <button
          disabled={excelDisabled}
          className={cl.excel}
          onClick={downloadExcel}
        >
          <FontAwesomeIcon icon={faFileExcel} />
          <p>{excelDisabled ? "Скачивание" : "Скачать Excel"}</p>
        </button>
        <button disabled={pdfDisabled} className={cl.pdf} onClick={downloadPdf}>
          <FontAwesomeIcon icon={faFilePdf} />
          <p>{pdfDisabled ? "Скачивание" : "Скачать PDF"}</p>
        </button>
      </div>
    </div>
  );
};
