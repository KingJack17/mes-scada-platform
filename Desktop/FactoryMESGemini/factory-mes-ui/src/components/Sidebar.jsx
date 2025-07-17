import React, { useState } from "react";
import { NavLink } from "react-router-dom";
import {
  Menu, X, Factory, Package, Wrench, Users, Settings, ServerCog, ClipboardList
} from "lucide-react";

const Sidebar = ({ isAdmin }) => {
  const [open, setOpen] = useState(false);

  const navItemClass = ({ isActive }) =>
    `flex items-center gap-3 p-2 rounded font-medium transition-colors duration-200 whitespace-nowrap overflow-hidden text-ellipsis ${
      isActive
        ? 'bg-blue-100 dark:bg-gray-700 text-blue-600'
        : 'text-gray-700 dark:text-gray-200 hover:text-blue-600'
    }`;

  return (
    <>
      {/* Mobil Menü Butonu */}
      <button
        className="md:hidden fixed top-4 left-4 z-50 bg-white dark:bg-gray-900 p-2 rounded shadow border border-gray-300 dark:border-gray-700"
        onClick={() => setOpen(!open)}
      >
        {open ? <X className="w-5 h-5" /> : <Menu className="w-5 h-5" />}
      </button>

      {/* Sidebar */}
      <aside className={`${open ? 'block' : 'hidden'} md:block fixed md:relative z-40 bg-white dark:bg-gray-900 w-64 h-screen p-4 shadow-xl border-r border-gray-200 dark:border-gray-800 transition-all duration-300 ease-in-out overflow-y-auto`}>
        
        {/* Logo */}
        <div className="flex justify-center mb-6">
          <img src="/logo.png" alt="Logo" className="h-10 w-auto" />
        </div>

        <nav className="space-y-3 text-sm">
          <NavLink to="/" className={navItemClass}>
            <Factory className="w-5 h-5" />
            Ana Sayfa
          </NavLink>
          <NavLink to="/products" className={navItemClass}>
            <Package className="w-5 h-5" />
            Ürünler
          </NavLink>
          <NavLink to="/machines" className={navItemClass}>
            <ServerCog className="w-5 h-5" />
            Makineler
          </NavLink>
          <NavLink to="/workorders" className={navItemClass}>
            <ClipboardList className="w-5 h-5" />
            İş Emirleri
          </NavLink>
          <NavLink to="/maintenance" className={navItemClass}>
            <Wrench className="w-5 h-5" />
            Bakım
          </NavLink>
          {isAdmin && (
            <NavLink to="/users" className={navItemClass}>
              <Users className="w-5 h-5" />
              Kullanıcılar
            </NavLink>
          )}
          <NavLink to="/settings" className={navItemClass}>
            <Settings className="w-5 h-5" />
            Ayarlar
          </NavLink>
        </nav>
      </aside>
    </>
  );
};

export default Sidebar;
