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
      <Link className={cl.logo} to="/projects">
        <img src={logo} alt="logo" />
      </Link>
      <ul className={cl.links}>
        <li
          className={window.location.pathname == "/projects" ? cl.active : ""}
        >
          <Link to="/projects">Проекты</Link>
        </li>
        <li className={window.location.pathname == "/reports" ? cl.active : ""}>
          <Link to="/reports">Отчёты</Link>
        </li>
      </ul>
      {/* profile */}
      <div
        className={[cl.profile, profileExpanded ? cl.expanded : ""].join(" ")}
        ref={dropdownRef}
        onClick={() => setProfileExpanded(!profileExpanded)}
      >
        <FontAwesomeIcon icon={faUser} />
        <span
          className={cl.profile_name}
        >{`${user?.firstName} ${user?.middleName}`}</span>
        <span className={cl.profile_mobile_name}>Профиль</span>
        <FontAwesomeIcon className={cl.chevron} icon={faChevronDown} />

        <ul
          className={[cl.expander_popup].join(" ")}
          onClick={(e) => e.stopPropagation()}
        >
          <span
            className={cl.mobile_title}
          >{`${user?.firstName} ${user?.middleName}`}</span>
          <span className={cl.mobile_separator}></span>
          <li>
            <button className={cl.popup_button} onClick={logout}>
              <FontAwesomeIcon icon={faSignOut} />
              <span>Выйти</span>
            </button>
          </li>
        </ul>
      </div>
    </nav>
  );
};

export default TopNavbar;
