import React, { useState, useRef, useEffect } from "react";
import { useNavigate } from "react-router-dom";
import ThemeToggle from "./ThemeToggle";
import { Bell, User } from "lucide-react";

const Header = ({ isAuthenticated = false, username = '', onLogout = () => {} }) => {
  const navigate = useNavigate();
  const [open, setOpen] = useState(false);
  const dropdownRef = useRef(null);

  useEffect(() => {
    const handleClickOutside = e => {
      if (dropdownRef.current && !dropdownRef.current.contains(e.target)) {
        setOpen(false);
      }
    };
    document.addEventListener("mousedown", handleClickOutside);
    return () => document.removeEventListener("mousedown", handleClickOutside);
  }, []);

  return (
    <header className="w-full bg-white dark:bg-gray-900 shadow-md border-b border-gray-200 dark:border-gray-800 px-4 py-3 flex flex-wrap items-center justify-between gap-3 transition-all duration-300">
      <h1 className="text-lg md:text-2xl font-bold text-gray-800 dark:text-white tracking-tight truncate">
        MES Dashboard
      </h1>

      <div className="flex items-center gap-3 relative text-sm font-medium min-w-0 flex-shrink-0">
        <ThemeToggle />
        <Bell className="text-gray-600 dark:text-gray-300 w-5 h-5" />

        <div className="relative" ref={dropdownRef}>
          <button
            onClick={() => {
              if (!isAuthenticated) {
                navigate("/login");
              } else {
                setOpen(!open);
              }
            }}
            className="flex items-center gap-2 text-gray-700 dark:text-gray-200"
          >
            <User className="w-5 h-5" />
            <span className="hidden sm:inline">{username || "Kullanıcı"}</span>
          </button>
          {isAuthenticated && open && (
            <div className="absolute right-0 mt-2 w-40 bg-white dark:bg-gray-800 border border-gray-200 dark:border-gray-700 rounded-md shadow-lg z-10">
              <button
                onClick={onLogout}
                className="w-full px-4 py-2 text-left text-sm text-red-600 dark:text-red-400 hover:bg-gray-100 dark:hover:bg-gray-700"
              >
                Çıkış Yap
              </button>
            </div>
          )}
        </div>
      </div>
    </header>
  );
};

export default Header;
