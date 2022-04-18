import React from 'react';
import ReactDOM from 'react-dom';
import Layout from './Components/Layout';
import { BrowserRouter, Routes, Route } from "react-router-dom";
import { render } from "react-dom";
import Login from "./Components/Login";
import Register from "./Components/Register";
import Home from "./Components/Home";

const rootElement = document.getElementById("root");
render(
  <BrowserRouter>
  <Layout/>
  </BrowserRouter>,
  rootElement
);