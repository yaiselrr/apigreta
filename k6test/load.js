import http from 'k6/http';
import { check, sleep } from 'k6';

const token = 'eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9.eyJpZCI6IjA2OGNiNzRhLWVjOTgtNDA5Yi04M2U5LWIxMGJhZWY0YjU0OCIsInVzZXJlbWFpbCI6ImNoZW5yeWhhYmFuYTIwNSttYW5hZ2VydGVzdGN1YmFAZ21haWwuY29tIiwidXNlciI6Im1hbmFnZXJkZXYiLCJ1c2Vycm9sZSI6ImRlZmluZVJvbGUiLCJzY29wZSI6InRlc3RjdWJhIiwibmJmIjoxNjg2NDI5ODc2LCJleHAiOjE2ODY0MzU4NzYsImlzcyI6Imh0dHBzOi8vaWRlbnRpdHkuZ3JldGF0ZXN0LmNvbSIsImF1ZCI6Imh0dHBzOi8vaWRlbnRpdHkuZ3JldGF0ZXN0LmNvbSJ9.hO8_jbaI8-wFSAEGo_6P3U0oDcm_FHv5poSNc0umOn0';
export const options = {
    insecureSkipTLSVerify: true,
    noConnectionReuse: false,
    stages: [
        // A list of virtual users { target: ..., duration: ... } objects that specify 
        // the target number of VUs to ramp up or down to for a specific period.
        { duration: '5m', target: 60 }, // simulate ramp-up of traffic from 1 to 100 users over 5 minutes.
        { duration: '5m', target: 60 }, // stay at 100 users for 10 minutes
        { duration: '5m', target: 0 }, // ramp-down to 0 users
    ],
    thresholds: {
        // A collection of threshold specifications to configure under what condition(s) 
        // a test is considered successful or not
        'http_req_duration': ['p(99)<9500'], // 99% of requests must complete below 1.5s
        //'logged in successfully': ['p(99)<1500'], // 99% of requests must complete below 1.5s
    }
};

export default function () {
    // Here, we set the endpoint to test.
    const response = http.get('http://localhost:5001/api/Product/1/50', {
        headers: {
            Authorization: `Bearer ${token}`,
        },
    });

    // An assertion
    check(response, {
        'is status 200': (x) => x.status === 200
    });

    sleep(1);
}