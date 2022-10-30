import cl from "./TopNavbar.module.scss";
import logo from "../../../assets/logos/logo.png";
import {
  faUser,
  faChevronDown,
  faSignOut,
} from "@fortawesome/free-solid-svg-icons";
import { FontAwesomeIcon } from "@fortawesome/react-fontawesome";
import { AuthContext } from "../../../api/AuthContext";
import { useContext, useState } from "react";
import { useOutsideAlerter } from "../../../hooks/useOutsideAlerter";
import { useRef } from "react";
import { Link } from "react-router-dom";

const TopNavbar = () => {
  const { user, logout } = useContext(AuthContext);
  const [profileExpanded, setProfileExpanded] = useState(false);
  const dropdownRef = useRef();
  useOutsideAlerter(dropdownRef, () => {
    setProfileExpanded(false);
  });

  return (
    <nav className={cl.navbar}>
      <Link className={cl.logo} to="/dashboard">
        <img src={logo} alt="logo" />
      </Link>
      <ul className={cl.links}>
        <li
          className={window.location.pathname == "/dashboard" ? cl.active : ""}
        >
          <Link to="/dashboard">Проекты</Link>
        </li>
        <li className={window.location.pathname == "/reports" ? cl.active : ""}>
          <Link to="/reports">Отчёты</Link>
        </li>
      </ul>
      {/* profile */}
      <button
        className={[cl.profile, profileExpanded ? cl.expanded : ""].join(" ")}
        ref={dropdownRef}
        onClick={() => setProfileExpanded(!profileExpanded)}
      >
        <FontAwesomeIcon icon={faUser} />
        <span>{`${user?.firstName} ${user?.lastName}`}</span>
        <FontAwesomeIcon className={cl.chevron} icon={faChevronDown} />

        <ul className={[cl.expander_popup].join(" ")}>
          <li>
            <div className={cl.popup_button}>
              <FontAwesomeIcon icon={faSignOut} />
              <span onClick={logout}>Выйти</span>
            </div>
          </li>
        </ul>
      </button>
    </nav>
  );
};

export default TopNavbar;
