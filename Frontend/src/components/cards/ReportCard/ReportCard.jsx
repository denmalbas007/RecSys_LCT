import cl from "./ReportCard.module.scss";
import { FontAwesomeIcon } from "@fortawesome/react-fontawesome";
import {
  faFilePdf,
  faFileExcel,
  faClock,
} from "@fortawesome/free-solid-svg-icons";

export const ReportCard = ({ projectTitle, pending, time }) => {
  return (
    <div className={cl.report_card}>
      <div className={cl.header}>
        <h3>{`Отчет по проекту "${projectTitle}"`}</h3>
        <div className={cl.date}>
          <FontAwesomeIcon icon={faClock} />
          <p>Запрошено в: 14:57</p>
        </div>
      </div>
      <div className={cl.body}>
        <button className={cl.excel}>
          <FontAwesomeIcon icon={faFileExcel} />
          <p>Скачать Excel</p>
        </button>
        <button className={cl.pdf}>
          <FontAwesomeIcon icon={faFilePdf} />
          <p>Скачать PDF</p>
        </button>
      </div>
    </div>
  );
};
