import React from 'react';
import PropTypes from 'prop-types';

const DateFormat = ({ dateString }) => {
    const formatDate = (dateString) => {
        const date = new Date(dateString);
        const options = {
            day: '2-digit',
            month: '2-digit',
            year: '2-digit',
            hour: '2-digit',
            minute: '2-digit',
            second: '2-digit',
            hour12: true
        };

        const formattedDate = new Intl.DateTimeFormat('en-GB', options).format(date);

        // Fix the hour part to include A.M./P.M. properly
        const parts = formattedDate.split(', ');
        const datePart = parts[0].split('/').reverse().join('-');
        const timePart = parts[1].replace(' at', '').replace('pm', 'PM').replace('am', 'AM');

        return `${datePart} ${timePart}`;
    };

    return (
        <span>{formatDate(dateString)}</span>
    );
};

DateFormat.propTypes = {
    dateString: PropTypes.string.isRequired
};

export default DateFormat;
