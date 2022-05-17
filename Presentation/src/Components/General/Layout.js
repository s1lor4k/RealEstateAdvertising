import React, {useState, useRef, useEffect} from 'react'
import 'bootstrap/dist/css/bootstrap.min.css';
import '../../Styles/index.css'
import '../../Styles/Layout.css'
import Topbar from './Topbar';
import Login from "../Onboarding/Login";
import Register from "../Onboarding/Register";
import Home from "../Home/Home";
import AddAdvertisement from '../Advertisements/AddAdvertisement';
import MyAdvertisements from '../Advertisements/MyAdvertisements';
import {Navigate, Route, Routes} from "react-router-dom";
import AuthService from '../../Services/AuthService';
import {ErrorBoundary} from 'react-error-boundary'
import AdvertisementService from "../../Services/AdvertisementService";

export default function Layout() {

    const [currentUser, setCurrentUser] = useState(undefined);
    const [ads, setAds] = useState({
        totalPages:0,
        pageSize:2,
        totalRecordsCount:0,
        items:[],
        currentPage:1
    });

    const [filters, setFilters] = useState({
        minPrice: 1000,
        maxPrice: 500000,
        cityId: 0,
        type: 0,
        isRent:null
    })

    useEffect(async () => {
        await getAds()
    },[ads.currentPage]);

    useEffect(async () => {
        setCurrentUser(AuthService.getCurrentUser())
    },[])

    async function handleAuthenticationUpdated() {
        setCurrentUser(AuthService.getCurrentUser())
        await getAds();
    }

    async function getAds(getStartingPage = false) {
        let tempAds = await AdvertisementService.getAllAdvertisments(filters,
            getStartingPage ? 1 : ads.currentPage,
             ads.pageSize
        );
        setAds(tempAds);
    }

    function ErrorFallback({error, resetErrorBoundary}) {
        return (
            <div role="alert">
                <p>Something went wrong:</p>
                <pre>{error.message}</pre>
                <button onClick={resetErrorBoundary}>Try again</button>
            </div>
        )
    }

    return (
        <div>
            <Topbar authenticationUpdated={handleAuthenticationUpdated}/>
            <div className="container mt-3">
                <Routes>
                    <Route index path={"/home"} element={<Home ads={ads} getAds={getAds} filters={filters} setFilters={setFilters} setAds={setAds}/>}/>
                    <Route path={"/my-advertisements"} element={
                        currentUser
                            ? (
                                <ErrorBoundary FallbackComponent={ErrorFallback}>
                                    <MyAdvertisements/>
                                </ErrorBoundary>
                            )
                            : (<Navigate to="/home" replace/>)}/>
                    <Route path={"/add-advertisement"} element={
                        currentUser
                            ? (
                                <ErrorBoundary FallbackComponent={ErrorFallback}>
                                    <AddAdvertisement/>
                                </ErrorBoundary>
                            )
                            : (<Navigate to="/home" replace/>)}/>
                    <Route path="*" element={<Navigate to="/home" replace/>}/>
                    <Route path="/login" element={
                        !currentUser
                            ? (
                                <ErrorBoundary FallbackComponent={ErrorFallback}>
                                    <Login authenticationUpdated={handleAuthenticationUpdated}/>
                                </ErrorBoundary>
                            )
                            : (<Navigate to="/home" replace/>)}/>
                    <Route path="/register" element={
                        !currentUser
                            ? (
                                <ErrorBoundary FallbackComponent={ErrorFallback}>
                                    <Register authenticationUpdated={handleAuthenticationUpdated}/>
                                </ErrorBoundary>
                            )
                            : (<Navigate to="/home" replace/>)}/>
                </Routes>
            </div>
        </div>
    )
}
